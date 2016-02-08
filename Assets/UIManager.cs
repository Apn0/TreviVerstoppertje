using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private Slider hpbar;
    private int playerhp;
	// Update is called once per frame
	void Start () {
        hpbar = GameObject.Find("PlayerUI").GetComponentInChildren<Slider>();
    }
    public void UpdatePlayerHealth (int hp)
    {
        hpbar.value = hp;
    }
}
