# TreviVerstoppertje

This project is a Unity game. A Python script is provided to extract all texture files from the `Assets` directory and optionally upscale them. The script relies on Pillow for basic resizing, and attempts to use [Real-ESRGAN](https://github.com/xinntao/Real-ESRGAN) if the package and model weights are available.

## Project Structure / Entry Points

- `Assets/` and `ProjectSettings/` contain the Unity game data and editor settings.  
  **How to run:** open the repository root in the Unity editor and press Play.
- `TreviShooter/` is currently empty in this repo, so it appears intended as a placeholder for a separate Unity subproject or module that has not been checked in yet.  
  **How to run:** if/when this folder is populated with a Unity project, open it in Unity as its own project.
- `index.html`, `script.js`, and `tour.js` appear to be web assets bundled with the repository.  
  **How to run:** open `index.html` in a browser, or serve the repo with a local web server if the scripts require hosting behavior.

See [Extract and Upscale Textures](#extract-and-upscale-textures) for the texture tooling.

## Tools

### Extract and Upscale Textures

```bash
python tools/extract_and_upscale_textures.py --assets Assets --output UpscaledTextures
```

By default, textures are upscaled using bicubic interpolation. Place a `RealESRGAN_x2.pth` weight file next to the script to enable machine-learning based upscaling.
