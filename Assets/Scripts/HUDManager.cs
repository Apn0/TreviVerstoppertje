using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
    private Camera cam;
    private Player player;

    [SerializeField]
    private GUISkin tagSkin;

    void Start()
    {

        cam = (PlayerSetup.localPlayer == null) ? null : PlayerSetup.localPlayer.GetComponentInChildren<Camera>();
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
        GUI.skin = tagSkin;
        foreach (System.Collections.Generic.KeyValuePair<string, Player> kvp in GameManager.GetPlayers())
        {
            player = kvp.Value;

            Vector3 pos = cam.WorldToViewportPoint(player.transform.position + new Vector3(0, 2.5f, 0));
            GUI.Label(new Rect(Screen.width*pos.x-75, Screen.height*(1-pos.y), 150, 20), player.GetComponent<Player>().playerName);
        }
    }
}
