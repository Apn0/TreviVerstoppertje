using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[ClientRpc]
	public static void RpcKill(GameObject player){
		Destroy (player);
	}
}
