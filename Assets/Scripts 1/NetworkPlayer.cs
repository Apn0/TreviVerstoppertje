using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<FirstPersonController>().enabled = true;
			GetComponentInChildren<Camera>().enabled = true;
			GetComponentInChildren<AudioListener>().enabled = true;
			GameObject.FindWithTag("SceneCam").SetActive(false);
		}
	}
}
