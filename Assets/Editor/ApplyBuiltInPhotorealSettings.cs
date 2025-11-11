using UnityEngine;
using UnityEditor;

public class ApplyBuiltInPhotorealSettings : Editor
{
    [MenuItem("Trevi/Apply Built-in Photoreal Settings")]
    public static void ApplySettings()
    {
        // Set Color Space to Linear
        PlayerSettings.colorSpace = ColorSpace.Linear;

        // Enable Deferred Shading
        // In Unity 5.3, this is controlled by the GraphicsSettings asset.
        // We will modify it using SerializedObject to ensure the change is saved.
        Object graphicsSettings = AssetDatabase.LoadAssetAtPath("ProjectSettings/GraphicsSettings.asset", typeof(Object));
        if (graphicsSettings != null)
        {
            SerializedObject serializedGraphicsSettings = new SerializedObject(graphicsSettings);

            // Find the property for Deferred Shading mode.
            // Based on the .asset file, the property is m_Deferred.m_Mode.
            // Value 1 = On, 2 = Off, 3 = Auto. We'll set it to 1.
            SerializedProperty deferredMode = serializedGraphicsSettings.FindProperty("m_Deferred.m_Mode");
            if (deferredMode != null)
            {
                deferredMode.intValue = 1; // 1 means On
            }
            else
            {
                Debug.LogError("Could not find the 'm_Deferred.m_Mode' property in GraphicsSettings. It might have changed in this Unity version.");
            }

            // Also try to set the legacy deferred path, just in case.
            SerializedProperty legacyDeferredMode = serializedGraphicsSettings.FindProperty("m_LegacyDeferred.m_Mode");
            if (legacyDeferredMode != null)
            {
                legacyDeferredMode.intValue = 1; // 1 means On
            }

            serializedGraphicsSettings.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("Graphics Settings", "Project graphics settings have been updated:\n\n- Color Space: Linear\n- Rendering Path: Deferred", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Could not load the GraphicsSettings.asset file.", "OK");
        }
    }
}
