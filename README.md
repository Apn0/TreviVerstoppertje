# TreviVerstoppertje

This project is a Unity game. A Python script is provided to extract all texture files from the `Assets` directory and optionally upscale them. The script relies on Pillow for basic resizing, and attempts to use [Real-ESRGAN](https://github.com/xinntao/Real-ESRGAN) if the package and model weights are available.

## TODO

- [x] Add a pinned requirements file for texture tooling so setup is one command.
- [ ] Add CLI options for scale factor and interpolation mode in `extract_and_upscale_textures.py`.
- [x] Add a dry-run mode that reports how many textures would be processed.
- [x] Add an option to skip files that already exist in the output directory.

## Project Structure / Entry Points

- `Assets/` and `ProjectSettings/` contain the Unity game data and editor settings.  
  **How to run:** open the repository root in the Unity editor and press Play.
- `TreviShooter/` is currently empty in this repo, so it appears intended as a placeholder for a separate Unity subproject or module that has not been checked in yet.  
  **How to run:** if/when this folder is populated with a Unity project, open it in Unity as its own project.
- `index.html`, `script.js`, and `tour.js` appear to be web assets bundled with the repository.  
  **How to run:** open `index.html` in a browser, or serve the repo with a local web server if the scripts require hosting behavior.

See [Extract and Upscale Textures](#extract-and-upscale-textures) for the texture tooling.

## Web Assets

The repository includes static web assets (`index.html`, `script.js`, `tour.js`). You can open
`index.html` directly in a browser for a quick check. If the browser blocks local file access
(for example due to CORS restrictions), run a local server from the repo root:

```bash
python -m http.server 8000
```

Then visit `http://localhost:8000/index.html`.

## Tools

### Extract and Upscale Textures

```bash
python tools/extract_and_upscale_textures.py --assets Assets --output UpscaledTextures
```

To avoid reprocessing files already in the output directory:

```bash
python tools/extract_and_upscale_textures.py --assets Assets --output UpscaledTextures --skip-existing
```

To preview work without writing output files:

```bash
python tools/extract_and_upscale_textures.py --assets Assets --output UpscaledTextures --dry-run
```

Requirements:
- Python 3.x
- Pillow (`pip install -r tools/requirements-textures.txt`)
- Optional: Real-ESRGAN + PyTorch for ML-based upscaling (`pip install realesrgan torch`)

By default, textures are upscaled using bicubic interpolation. Place a `RealESRGAN_x2.pth` weight file next to the script to enable machine-learning based upscaling.

You can obtain `RealESRGAN_x2.pth` from the Real-ESRGAN release assets: https://github.com/xinntao/Real-ESRGAN/releases
