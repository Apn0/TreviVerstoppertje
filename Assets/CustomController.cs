using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour {
    
    public float groundCheckDistance;
    public float abovestart;

    private Vector3 relative;
    private Vector3 startpos;
    private Animator anim;
    private bool isGrounded;
    private CharacterController cc;

    int llayerMask = 1 << 8;

    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        llayerMask = ~llayerMask;
    }
	
	// Update is called once per frame
	void Update () {

        relative = transform.InverseTransformDirection(cc.velocity);
        // Debug.Log(relative);


        checkGrounded();

        // if (cc.isGrounded | NetworkScript.grounded)
        if (isGrounded)
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
    void checkGrounded()
    {
        startpos = transform.position + new Vector3(0, abovestart, 0);

        RaycastHit hit;
        if (Physics.Raycast(startpos, -Vector3.up, out hit, maxDistance: (groundCheckDistance + abovestart), layerMask:llayerMask))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    
}
