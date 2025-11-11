#!/bin/bash

# This script downloads the 30 photo spheres for the TreviShooter project.
# Please run it from the root directory of the project.

# Create the target directory if it doesn't exist
mkdir -p "Assets/Trevi/Source/PhotoSpheres"

# Base URL for the images
BASE_URL="http://Oranje-Eco.com/static/Street%20View%20360"

# Download the first image (which has no number suffix)
echo "Downloading Street View 360.jpg..."
curl -L -o "Assets/Trevi/Source/PhotoSpheres/Street View 360.jpg" "${BASE_URL}.jpg"

# Download the numbered images from 2 to 30
for i in {2..30}; do
  FILENAME="Street View 360_${i}.jpg"
  URL="${BASE_URL}_${i}.jpg"
  echo "Downloading ${FILENAME}..."
  curl -L -o "Assets/Trevi/Source/PhotoSpheres/${FILENAME}" "${URL}"
done

echo "Download complete! All 30 photo spheres should now be in the Assets/Trevi/Source/PhotoSpheres/ directory."
