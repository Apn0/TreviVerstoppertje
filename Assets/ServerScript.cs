using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	    if(!isServer)
        {
            enabled = false;
        }
	}
	
    
}
