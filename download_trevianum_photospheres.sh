#!/bin/bash
# download_trevianum_photospheres.sh
# This script downloads photosphere tiles from naartrevianum.nl

BASE_URL="https://naartrevianum.nl/media/"
DEST_DIR="Assets/StreamingAssets/TrevianumPhotospheres"

# Create the destination directory if it doesn't exist
mkdir -p "$DEST_DIR"

# Read the list of panorama base URLs
while IFS= read -r path_template; do
    # Extract panorama name and resolution level from the path
    # e.g., media/panorama_BA5E9A93_B39F_D28D_41D2_5174D021287A_0/{face}/0/{row}_{column}.jpg
    
    if [[ $path_template =~ media\/(panorama_[[:alnum:]_]+)\/\{face\}\/([0-3])\/\{row\}_\{column\}\.jpg ]]; then
        panorama_name="${BASH_REMATCH[1]}"
        resolution_level="${BASH_REMATCH[2]}"
    else
        echo "Skipping malformed URL template: $path_template"
        continue
    fi

    # Determine row and column counts based on resolution level
    case $resolution_level in
        0) row_count=7; col_count=42 ;;
        1) row_count=4; col_count=24 ;;
        2) row_count=2; col_count=12 ;;
        3) row_count=1; col_count=6 ;;
        *) echo "Unknown resolution level: $resolution_level"; continue ;;
    esac

    echo "Downloading panorama: $panorama_name at resolution level $resolution_level"

    # Create directories for the panorama and resolution level
    mkdir -p "$DEST_DIR/$panorama_name/$resolution_level"

    # Iterate over cube faces
    for face in f b l r u d; do
        # Create directory for the face
        mkdir -p "$DEST_DIR/$panorama_name/$resolution_level/$face"
        
        # Iterate over rows and columns to download each tile
        for (( row=0; row<row_count; row++ )); do
            for (( col=0; col<col_count; col++ )); do
                tile_url_path=$(echo "$path_template" | sed -e "s/{face}/$face/" -e "s/{row}/$row/" -e "s/{column}/$col/")
                tile_dest_path="$DEST_DIR/$panorama_name/$resolution_level/$face/${row}_${col}.jpg"
                
                # Check if file already exists to avoid re-downloading
                if [ ! -f "$tile_dest_path" ]; then
                    echo "Downloading $tile_url_path to $tile_dest_path"
                    curl -s -o "$tile_dest_path" "https://naartrevianum.nl/$tile_url_path"
                else
                    echo "Skipping existing file: $tile_dest_path"
                fi
            done
        done
    done

done < photosphere_urls.txt

echo "All photosphere tiles downloaded."
