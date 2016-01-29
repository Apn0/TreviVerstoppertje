using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkPlayer : NetworkBehaviour {
	[SerializeField]
	Behaviour[] ComponentsToDisable;

	GameObject sceneCam;


	// Use this for initialization
	void Start () {
		sceneCam = GameObject.FindWithTag ("SceneCam");
		if (isLocalPlayer) {
			sceneCam.SetActive(false);
		} else {
			GetComponent<FirstPersonController>().enabled = false;
			GetComponentInChildren<Camera>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;
			this.tag = "otherPlayer";
			GetComponent<Scht>().enabled = false;
		}
	}

	void OnDestroy() {
		if (isLocalPlayer) {
			sceneCam.SetActive (true);
		}
	}
}
