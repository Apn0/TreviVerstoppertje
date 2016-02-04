using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour {
	[SyncVar]
	public int playerHealth;

	// Use this for initialization

}
