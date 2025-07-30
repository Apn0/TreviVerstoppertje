# TreviVerstoppertje

This project is a Unity game. A Python script is provided to extract all texture files from the `Assets` directory and optionally upscale them. The script relies on Pillow for basic resizing, and attempts to use [Real-ESRGAN](https://github.com/xinntao/Real-ESRGAN) if the package and model weights are available.

## Tools

### Extract and Upscale Textures

```bash
python tools/extract_and_upscale_textures.py --assets Assets --output UpscaledTextures
```

By default, textures are upscaled using bicubic interpolation. Place a `RealESRGAN_x2.pth` weight file next to the script to enable machine-learning based upscaling.
