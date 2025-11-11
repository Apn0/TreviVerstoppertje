using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateSkyboxMaterials : EditorWindow
{
    [MenuItem("Trevi/Create Skybox Materials")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateSkyboxMaterials));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Skyboxes"))
        {
            CreateMaterials();
        }
    }

    private void CreateMaterials()
    {
        string sourcePath = "Assets/Trevi/Source/PhotoSpheres";
        string destPath = "Assets/Trevi/Materials/Skyboxes";

        if (!Directory.Exists(destPath))
        {
            Directory.CreateDirectory(destPath);
        }

        string[] imageFiles = Directory.GetFiles(sourcePath, "*.jpg");

        foreach (string imageFile in imageFiles)
        {
            // We need to configure the texture importer first
            TextureImporter tImporter = AssetImporter.GetAtPath(imageFile) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.textureType = TextureImporterType.Default;
                tImporter.npotScale = TextureImporterNPOTScale.None;
                tImporter.wrapMode = TextureWrapMode.Clamp;
                tImporter.isReadable = true;
                AssetDatabase.ImportAsset(imageFile, ImportAssetOptions.ForceUpdate);
            }

            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(imageFile);

            if (texture != null)
            {
                string materialName = Path.GetFileNameWithoutExtension(imageFile);
                Material skyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));
                skyboxMaterial.SetTexture("_MainTex", texture);
                AssetDatabase.CreateAsset(skyboxMaterial, destPath + "/" + materialName + ".mat");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Skybox Creation", "Successfully created " + imageFiles.Length + " skybox materials.", "OK");
    }
}
