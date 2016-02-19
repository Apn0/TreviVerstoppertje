using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
	// Update is called once per frame

    public void UpdatePlayerHealth (int hp)
    {
        if (hpbar == null)
        {
            hpbar = GetComponent<PlayerSetup>().playerUIInstance.GetComponentInChildren<Slider>();
        }
        hpbar.value = hp;
    }
}
