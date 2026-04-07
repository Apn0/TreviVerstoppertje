import argparse
from pathlib import Path
from PIL import Image

try:
    from realesrgan import RealESRGAN
    import torch
    REAL_ESRGAN_AVAILABLE = True
except Exception:
    REAL_ESRGAN_AVAILABLE = False


SUPPORTED_EXTENSIONS = {'.png', '.jpg', '.jpeg', '.tga', '.bmp', '.tif', '.tiff'}


def find_textures(asset_dir: Path):
    for path in asset_dir.rglob('*'):
        if path.is_file() and path.suffix.lower() in SUPPORTED_EXTENSIONS:
            yield path


def upscale_image(input_path: Path, output_path: Path):
    img = Image.open(input_path)
    img = img.convert('RGB')
    if REAL_ESRGAN_AVAILABLE:
        device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
        model = RealESRGAN(device, scale=2)
        try:
            model.load_weights(Path(__file__).with_name('RealESRGAN_x2.pth'))
            upscaled = model.predict(img)
            upscaled.save(output_path)
            return
        except Exception as e:
            print(f'Failed to use RealESRGAN on {input_path}: {e}. Falling back to bicubic resize.')
    upscaled = img.resize((img.width * 2, img.height * 2), Image.BICUBIC)
    upscaled.save(output_path)


def main():
    parser = argparse.ArgumentParser(description='Extract and upscale Unity textures')
    parser.add_argument('--assets', default='Assets', help='Root assets directory')
    parser.add_argument('--output', default='UpscaledTextures', help='Directory to store upscaled textures')
    parser.add_argument(
        '--skip-existing',
        action='store_true',
        help='Skip output files that already exist in the target directory',
    )
    args = parser.parse_args()

    assets_dir = Path(args.assets)
    if not assets_dir.exists():
        raise SystemExit(f'Assets directory does not exist: {assets_dir}')
    output_dir = Path(args.output)
    output_dir.mkdir(parents=True, exist_ok=True)

    textures = list(find_textures(assets_dir))
    print(f'Found {len(textures)} texture files.')
    skipped = 0

    for tex in textures:
        rel = tex.relative_to(assets_dir)
        target_path = output_dir / rel
        if args.skip_existing and target_path.exists():
            skipped += 1
            print(f'Skipped existing {target_path}')
            continue
        target_path.parent.mkdir(parents=True, exist_ok=True)
        upscale_image(tex, target_path)
        print(f'Processed {tex} -> {target_path}')

    if args.skip_existing:
        print(f'Skipped {skipped} existing files.')


if __name__ == '__main__':
    main()
