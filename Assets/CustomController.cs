using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour {
    private CharacterController cc;

    public Vector3 relative;
    private Animator anim;
    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        relative = transform.InverseTransformDirection(cc.velocity);
        Debug.Log(relative);


        if (cc.isGrounded | NetworkScript.grounded)
        {
            anim.SetBool("Grounded", true);
            anim.SetFloat("xspeed", relative.x / 5);
            anim.SetFloat("zspeed", relative.z / 5);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }
        anim.SetFloat("yspeed", relative.y);
    }

    
}
