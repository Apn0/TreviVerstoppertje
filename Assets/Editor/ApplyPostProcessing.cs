using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ApplyPostProcessing : EditorWindow
{
    private string tonemappingName = "Tonemapping";
    private string bloomName = "Bloom";
    private string ssaoName = "SSAO";
    private string colorCorrectionName = "ColorCorrectionCurves";

    [MenuItem("Trevi/Apply Post-Processing")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ApplyPostProcessing));
    }

    void OnGUI()
    {
        GUILayout.Label("Post-Processing Component Names", EditorStyles.boldLabel);
        tonemappingName = EditorGUILayout.TextField("Tonemapping", tonemappingName);
        bloomName = EditorGUILayout.TextField("Bloom", bloomName);
        ssaoName = EditorGUILayout.TextField("SSAO", ssaoName);
        colorCorrectionName = EditorGUILayout.TextField("Color Correction", colorCorrectionName);

        if (GUILayout.Button("Apply"))
        {
            ApplyEffects();
        }
    }

    private void ApplyEffects()
    {
        // Open the Skybox scene
        string scenePath = "Assets/Trevi/Scenes/SkyboxScene.unity";
        EditorSceneManager.OpenScene(scenePath);

        // Find the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Add the specified components by name
            AddComponentByName(mainCamera.gameObject, tonemappingName);
            AddComponentByName(mainCamera.gameObject, bloomName);
            AddComponentByName(mainCamera.gameObject, ssaoName);
            AddComponentByName(mainCamera.gameObject, colorCorrectionName);

            EditorUtility.DisplayDialog("Post-Processing Setup", "The specified post-processing components have been added to the Main Camera.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Could not find the Main Camera in the scene.", "OK");
        }
    }

    private void AddComponentByName(GameObject obj, string componentName)
    {
        System.Type type = System.Type.GetType(componentName + ", UnityEngine.StandardAssets.ImageEffects");
        if (type != null)
        {
            obj.AddComponent(type);
        }
        else
        {
            Debug.LogError("Could not find a component with the name: " + componentName);
        }
    }
}
