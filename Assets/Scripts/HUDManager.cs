using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
    private Camera cam;
	// Update is called once per frame

    public void UpdatePlayerHealth (int hp)
    {
        if (hpbar == null)
        {
            hpbar = GetComponent<PlayerSetup>().playerUIInstance.GetComponentInChildren<Slider>();
        }
        hpbar.value = hp;
    }

    void onGui()
    {
        foreach (System.Collections.Generic.KeyValuePair<string, Player> kvp in GameManager.GetPlayers())
        {
            cam = kvp.Value.GetComponentInChildren<Camera>();
            GUI.color = Color.green;
            Vector3 pos = cam.WorldToViewportPoint(kvp.Value.transform.position);
            Vector3 pos1 = kvp.Value.transform.position = cam.transform.position;
            if(Vector3.Dot(cam.transform.forward, pos1) > 0)
            {
                GUI.Label(new Rect(pos.x, (Screen.height - pos.y) - pos.z / cam.fieldOfView, 155, 155), kvp.Value.transform.name);
            }
            GUI.color = Color.black;
        }
    }
}
