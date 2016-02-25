using UnityEngine;
using System.Collections;

public class EscListener : MonoBehaviour {
	GameObject guiCanvas;
    
	// Use this for initialization
	void Start () {
        guiCanvas = GameObject.Find("GUICanvas");
		guiCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			if(guiCanvas.activeSelf){
				guiCanvas.SetActive (false);
                Cursor.visible = false;
                GameManager.Test();
			}else{
				guiCanvas.SetActive(true);
                Cursor.visible = true;
			}
		}
	}
}
