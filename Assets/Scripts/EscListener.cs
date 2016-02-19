using UnityEngine;
using System.Collections;

public class EscListener : MonoBehaviour {
	GameObject guiCanvas;
	// Use this for initialization
	void Start () {
		guiCanvas = GameObject.Find ("Canvas");
		guiCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			if(guiCanvas.active){
				guiCanvas.SetActive (false);
			}else{
				guiCanvas.SetActive(true);
			}
		}
	}
}
