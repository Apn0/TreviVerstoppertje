import unittest
from unittest.mock import MagicMock, patch
import sys
from pathlib import Path
import tempfile
import shutil

# Mock PIL before importing the script
mock_pil = MagicMock()
sys.modules['PIL'] = mock_pil
sys.modules['PIL.Image'] = mock_pil.Image

import tools.extract_and_upscale_textures as script

class TestFindTextures(unittest.TestCase):
    def setUp(self):
        self.test_dir = Path(tempfile.mkdtemp())

    def tearDown(self):
        shutil.rmtree(self.test_dir)

    def test_find_textures_supported(self):
        # Create supported files
        (self.test_dir / "test1.png").touch()
        (self.test_dir / "test2.JPG").touch()
        (self.test_dir / "subdir").mkdir()
        (self.test_dir / "subdir" / "test3.tga").touch()

        # Create unsupported files
        (self.test_dir / "test.txt").touch()
        (self.test_dir / "image.psd").touch()

        found = list(script.find_textures(self.test_dir))
        found_names = {p.name for p in found}

        self.assertEqual(len(found), 3)
        self.assertIn("test1.png", found_names)
        self.assertIn("test2.JPG", found_names)
        self.assertIn("test3.tga", found_names)
        self.assertNotIn("test.txt", found_names)

class TestUpscaleImage(unittest.TestCase):
    @patch('tools.extract_and_upscale_textures.REAL_ESRGAN_AVAILABLE', False)
    def test_upscale_image_bicubic_fallback(self):
        input_path = Path("input.png")
        output_path = Path("output.png")

        mock_image = MagicMock()
        mock_image.width = 100
        mock_image.height = 100
        mock_pil.Image.open.return_value = mock_image

        # mock_image.convert returns a NEW mock, so we need to set width/height on it too
        converted_image = mock_image.convert.return_value
        converted_image.width = 100
        converted_image.height = 100

        script.upscale_image(input_path, output_path)

        mock_pil.Image.open.assert_called_once_with(input_path)
        mock_image.convert.assert_called_once_with('RGB')

        # Check if resize was called with 2x dimensions
        # The result of convert('RGB') is what is resized
        converted_image = mock_image.convert.return_value
        converted_image.resize.assert_called_once()
        args, kwargs = converted_image.resize.call_args
        self.assertEqual(args[0], (200, 200))

        # Check if save was called on the upscaled image
        upscaled_image = converted_image.resize.return_value
        upscaled_image.save.assert_called_once_with(output_path)

class TestMain(unittest.TestCase):
    def setUp(self):
        self.test_assets = Path(tempfile.mkdtemp())
        self.test_output = Path(tempfile.mkdtemp())

    def tearDown(self):
        shutil.rmtree(self.test_assets)
        shutil.rmtree(self.test_output)

    @patch('sys.argv', ['script_name', '--assets', 'non_existent_dir'])
    def test_main_assets_not_found(self):
        with self.assertRaises(SystemExit):
            script.main()

    @patch('tools.extract_and_upscale_textures.upscale_image')
    def test_main_success(self, mock_upscale):
        # Create a texture
        (self.test_assets / "tex.png").touch()

        with patch('sys.argv', ['script_name', '--assets', str(self.test_assets), '--output', str(self.test_output)]):
            script.main()

        mock_upscale.assert_called_once()
        self.assertTrue((self.test_output / "tex.png").parent.exists())

    @patch('tools.extract_and_upscale_textures.upscale_image')
    def test_main_dry_run(self, mock_upscale):
        (self.test_assets / "tex.png").touch()

        with patch('sys.argv', ['script_name', '--assets', str(self.test_assets), '--output', str(self.test_output), '--dry-run']):
            script.main()

        mock_upscale.assert_not_called()
        self.assertFalse((self.test_output / "tex.png").exists())

    @patch('tools.extract_and_upscale_textures.upscale_image')
    def test_main_skip_existing(self, mock_upscale):
        (self.test_assets / "tex.png").touch()
        # Create existing output
        (self.test_output / "tex.png").touch()

        with patch('sys.argv', ['script_name', '--assets', str(self.test_assets), '--output', str(self.test_output), '--skip-existing']):
            script.main()

        mock_upscale.assert_not_called()

if __name__ == '__main__':
    unittest.main()
