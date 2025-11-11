using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
#if UNITY_5_3_OR_NEWER
using UnityEditorInternal;
#endif

public static class ApplyBuiltInPhotorealSettings
{
    [MenuItem("Trevi/Apply Photoreal Settings (Built-in)")]
    public static void Run()
    {
        // 1) Core project settings
        PlayerSettings.colorSpace = ColorSpace.Linear;
        try { QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable; } catch {}
        QualitySettings.antiAliasing = 4;            // MSAA for forward cams; harmless in deferred
        QualitySettings.masterTextureLimit = 0;      // Full-res textures
        QualitySettings.shadowDistance = 80f;
        QualitySettings.shadowResolution = ShadowResolution.High;

        // Prefer Deferred (set default for new cameras)
        try {
            var tiers = new[] { GraphicsTier.Tier1, GraphicsTier.Tier2, GraphicsTier.Tier3 };
            foreach (var t in tiers)
            {
#if UNITY_5_4_OR_NEWER
                var ts = EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Standalone, t);
                ts.renderingPath = RenderingPath.DeferredShading;
                ts.hdr = true;
                EditorGraphicsSettings.SetTierSettings(BuildTargetGroup.Standalone, t, ts);
#endif
            }
        } catch {}

        // 2) Scene-level lighting defaults
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        RenderSettings.reflectionIntensity = 1.0f;
        RenderSettings.reflectionBounces = 1;

        // Create/ensure a Directional Light named "Sun" if none exists
        Light sun = Object.FindObjectOfType<Light>();
        if (sun == null || sun.type != LightType.Directional)
        {
            var go = new GameObject("Sun");
            sun = go.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.2f;
            go.transform.rotation = Quaternion.Euler(45f, 30f, 0f);
        }
        else
        {
            sun.type = LightType.Directional;
            sun.intensity = Mathf.Max(1.0f, sun.intensity);
        }
        sun.shadows = LightShadows.Soft;

        // 3) Lightmapping (Enlighten in 5.3)
        try {
            LightmapEditorSettings.realtimeResolution = 2.0f;   // texels per unit
            LightmapEditorSettings.bakeResolution = 20.0f;      // baked texels per unit
            LightmapEditorSettings.aoMaxDistance = 1.0f;
            LightmapEditorSettings.aoExponentIndirect = 1.0f;
            LightmapEditorSettings.aoExponentDirect = 1.0f;
#if UNITY_5_4_OR_NEWER
            LightmapEditorSettings.lightmapParameters = null;   // default
#endif
        } catch {}

        // 4) Optional: create a coarse Reflection Probe grid helper under an empty if none exists
        if (Object.FindObjectOfType<ReflectionProbe>() == null)
        {
            var root = new GameObject("Trevi_ReflectionProbes");
            int nx = 3, nz = 2;
            float dx = 25f, dz = 25f, y = 6f;
            Vector3 origin = Vector3.zero;
            for (int ix=0; ix<nx; ix++)
            for (int iz=0; iz<nz; iz++)
            {
                var pgo = new GameObject(string.Format("Probe_{0}_{1}", ix, iz));
                pgo.transform.parent = root.transform;
                pgo.transform.position = origin + new Vector3(ix*dx, y, iz*dz);
                var rp = pgo.AddComponent<ReflectionProbe>();
                rp.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
                rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                rp.size = new Vector3(30f, 12f, 30f);
                rp.intensity = 1f;
                rp.boxProjection = true;
            }
        }

        // 5) Minimal texture-import heuristics for folder Assets/Trevi/Textures
        string[] search = new[] { "Assets/Trevi/Textures" };
        foreach (var guid in AssetDatabase.FindAssets("t:Texture", search))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti == null) continue;

            string name = System.IO.Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
            bool isNormal = name.Contains("normal") || name.EndsWith("_n") || name.Contains("_nrm");
            bool isAO     = name.Contains("ao") || name.Contains("occlusion");
            bool isMask   = name.Contains("metal") || name.Contains("rough") || name.Contains("smooth");

            ti.mipmapEnabled = true;
            ti.anisoLevel = 8;
            ti.textureCompression = TextureImporterCompression.CompressedHQ;

            if (isNormal)
            {
                ti.textureType = TextureImporterType.NormalMap;
                ti.sRGBTexture = false;
            }
            else if (isAO || isMask)
            {
                ti.textureType = TextureImporterType.Default;
                ti.sRGBTexture = false;
            }
            else
            {
                ti.textureType = TextureImporterType.Default;
                ti.sRGBTexture = true;
            }
            EditorUtility.SetDirty(ti);
            ti.SaveAndReimport();
        }

        // 6) Drop a verification marker
        string verDir = "Assets/Trevi/Validation";
        if (!AssetDatabase.IsValidFolder(verDir))
        {
            string[] parts = verDir.Split('/');
            string cur = parts[0];
            for (int i=1;i<parts.Length;i++)
            {
                string next = cur + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(cur, parts[i]);
                cur = next;
            }
        }
        System.IO.File.WriteAllText(System.IO.Path.Combine(verDir, "SETTINGS_APPLIED.txt"),
            "Linear/Deferred/Lighting defaults applied. " + System.DateTime.Now.ToString("s"));
        AssetDatabase.Refresh();

        Debug.Log("[Trevi] Built-in photoreal settings applied.");
    }
}
