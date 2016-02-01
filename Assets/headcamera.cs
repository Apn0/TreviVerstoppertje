using UnityEngine;
using System.Collections;

public class headcamera : MonoBehaviour {
    public GameObject camera;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = camera.transform.rotation;
	}
}
