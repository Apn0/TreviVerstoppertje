using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	[SyncVar]
	int health;


	
	// Update is called once per frame
	void Update () {
	
	}

	
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;
		
		health -= amount;
	}
}
