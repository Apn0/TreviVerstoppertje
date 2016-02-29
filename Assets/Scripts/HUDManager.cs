using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
    private Camera cam;
    private Player player;
	// Update is called once per frame

    public void UpdatePlayerHealth (int hp)
    {
        if (hpbar == null)
        {
            hpbar = GetComponent<PlayerSetup>().playerUIInstance.GetComponentInChildren<Slider>();
        }
        hpbar.value = hp;
    }

    void OnGUI()
    {
        foreach (System.Collections.Generic.KeyValuePair<string, Player> kvp in GameManager.GetPlayers())
        {
            player = kvp.Value;
            GUI.color = Color.red;

            Vector3 pos = cam.WorldToViewportPoint(player.transform.position);
            /*Vector3 pos1 = kvp.Value.transform.position - cam.transform.position;
            if(Vector3.Dot(cam.transform.forward, pos1) > 0)
            {
                GUI.Label(new Rect(pos.x, (Screen.height - pos.y) - pos.z / cam.fieldOfView, 155, 155), kvp.Value.transform.name);
            }*/

            //Debug.Log(pos.x + ", " + pos.y);
            GUI.Label(new Rect(pos.x, pos.y, 155, 155), player.GetComponent<Player>().playerName);

            GUI.color = Color.black;
        }
    }
}
