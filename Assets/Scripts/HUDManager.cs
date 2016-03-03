using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
    [SerializeField]
    private Camera cam;
    private Player player;
    private GUIStyle GUIStyle;
	// Update is called once per frame

    void Start()
    {
        GUIStyle = new GUIStyle();
        GUIStyle.alignment = TextAnchor.MiddleCenter;
    }

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
        cam = Camera.current;
        foreach (System.Collections.Generic.KeyValuePair<string, Player> kvp in GameManager.GetPlayers())
        {
            player = kvp.Value;
            GUI.color = Color.red;

            Vector3 pos = cam.WorldToViewportPoint(player.transform.position + new Vector3(0, 2.5f, 0));
            GUI.Label(new Rect(Screen.width*pos.x-75, Screen.height*(1-pos.y), 150, 20), player.GetComponent<Player>().playerName, GUIStyle);

            GUI.color = Color.black;
        }
    }
}
