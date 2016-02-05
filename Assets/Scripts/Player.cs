using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[ClientRpc]
	public void RpcKill(){
		Destroy (transform.gameObject);
	}
}
