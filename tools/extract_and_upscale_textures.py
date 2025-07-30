import argparse
from pathlib import Path
import shutil
from PIL import Image

try:
    from realesrgan import RealESRGAN
    import torch
    REAL_ESRGAN_AVAILABLE = True
except Exception:
    REAL_ESRGAN_AVAILABLE = False


SUPPORTED_EXTENSIONS = {'.png', '.jpg', '.jpeg', '.tga', '.bmp', '.tif', '.tiff'}


def find_textures(asset_dir: Path):
    for ext in SUPPORTED_EXTENSIONS:
        yield from asset_dir.rglob(f'*{ext}')


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
    args = parser.parse_args()

    assets_dir = Path(args.assets)
    output_dir = Path(args.output)
    output_dir.mkdir(parents=True, exist_ok=True)

    textures = list(find_textures(assets_dir))
    print(f'Found {len(textures)} texture files.')

    for tex in textures:
        rel = tex.relative_to(assets_dir)
        target_path = output_dir / rel
        target_path.parent.mkdir(parents=True, exist_ok=True)
        upscale_image(tex, target_path)
        print(f'Processed {tex} -> {target_path}')


if __name__ == '__main__':
    main()
