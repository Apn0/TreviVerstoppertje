using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Scht : NetworkBehaviour {
	private Camera camera;
	public GameObject flash;
	//Vector3 guntransform = transform.position;
	
	// Use this for initialization
	void Start() {
		camera = GetComponentInChildren<Camera> ();
		flash.SetActive(false);
		//damage aflezen van geweer in kinderen. 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			DoFire(0);

		}
		//if (Input.GetButtonDown ("Fire2")) {
		//	transform.position = guntransform - Vector3(-2.0f, 0.0f, 0.0f);
		//}
	}

	[Client]
	void DoFire(float lifeTime)
	{
			
			//Schiet over internet!
		Debug.Log("Ik schiet!");

			//Instantiate(flash, this.transform.position, this.transform.rotation);
			Ray ray = camera.ViewportPointToRay (new Vector3 (0.5F, 0.5F, 0));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "otherPlayer") {
					CmdDamage (hit.transform.gameObject);
				}
			} else {
				print ("I'm looking at nothing!");
			}

	}

	[Command]
	void CmdDamage(GameObject player) {
		Player.RpcKill(player)
	}
}
