using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;

public class CreateSkyboxScene : EditorWindow
{
    [MenuItem("Trevi/Create Skybox Scene")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateSkyboxScene));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Scene"))
        {
            CreateScene();
        }
    }

    private void CreateScene()
    {
        // Create a new scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // --- Skybox Manager Setup ---
        GameObject skyboxManagerObject = new GameObject("SkyboxManager");
        SkyboxManager skyboxManager = skyboxManagerObject.AddComponent<SkyboxManager>();

        string skyboxMaterialPath = "Assets/Trevi/Materials/Skyboxes";
        string[] materialFiles = Directory.GetFiles(skyboxMaterialPath, "*.mat");
        skyboxManager.skyboxes = new Material[materialFiles.Length];
        for (int i = 0; i < materialFiles.Length; i++)
        {
            skyboxManager.skyboxes[i] = (Material)AssetDatabase.LoadAssetAtPath(materialFiles[i], typeof(Material));
        }

        // --- Lighting Setup ---
        // Create Sun Light
        GameObject sunObject = new GameObject("Sun");
        Light sunLight = sunObject.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.color = new Color(1f, 0.95f, 0.84f); // Warm sun color
        sunLight.intensity = 1.0f;
        sunObject.transform.rotation = Quaternion.Euler(50, -30, 0);

        // Create Reflection Probe
        GameObject probeObject = new GameObject("ReflectionProbe");
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = ReflectionProbeMode.Realtime;
        probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting; // Or OnAwake if you prefer

        // Configure Scene Lighting Settings
        if (skyboxManager.skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxManager.skyboxes[0];
        }
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

        // Enable Enlighten GI
        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;

        // Save the scene
        string scenePath = "Assets/Trevi/Scenes/SkyboxScene.unity";
        EditorSceneManager.SaveScene(newScene, scenePath);

        EditorUtility.DisplayDialog("Scene Creation", "Successfully created the Skybox scene at " + scenePath + " with a complete lighting setup.", "OK");
    }
}
