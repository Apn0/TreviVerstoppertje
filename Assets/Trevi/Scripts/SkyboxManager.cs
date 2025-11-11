using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material[] skyboxes;
    private int currentIndex = 0;

    void Start()
    {
        if (skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxes[currentIndex];
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Next Skybox"))
        {
            currentIndex = (currentIndex + 1) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[currentIndex];
        }

        if (GUI.Button(new Rect(10, 50, 150, 30), "Previous Skybox"))
        {
            currentIndex = (currentIndex - 1 + skyboxes.Length) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[currentIndex];
        }
    }
}
