using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AnimationApplier : NetworkBehaviour
{

    public float groundCheckDistance;
    public float abovestart;

    public Transform myCamera;
    public Transform headBone;

    public Vector3 headBoneRot;

    private Vector3 relative;
    private Vector3 startpos;
    private Animator anim;
    private bool isGrounded;
    private CharacterController cc;


    int llayerMask = 1 << 8;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        llayerMask = ~llayerMask;
    }


    void Update()
    {

        relative = transform.InverseTransformDirection(cc.velocity);
        checkGrounded();

        if (isGrounded)
        {
            anim.SetBool("Grounded", true);
            if (isLocalPlayer)
            {
                anim.SetFloat("xspeed", relative.x / 5);
                anim.SetFloat("zspeed", relative.z / 5);
            }
        }
        else
        {
            anim.SetBool("Grounded", false);
        }
        anim.SetFloat("yspeed", relative.y);


    }
    void LateUpdate()
    {
        headBoneRot = new Vector3(myCamera.localEulerAngles.x, headBone.localEulerAngles.y, headBone.localEulerAngles.z);
        headBone.localEulerAngles = headBoneRot;

        //1. verander waarde heupen. 

        //2. verander waarde hoofd.

        //3. verander waarde armen.


    }
    void checkGrounded()
    {
        startpos = transform.position + new Vector3(0, abovestart, 0);

        RaycastHit hit;
        if (Physics.Raycast(startpos, -Vector3.up, out hit, maxDistance: (groundCheckDistance + abovestart), layerMask: llayerMask))
        {
            isGrounded = (hit.collider.tag == "Ground") ? true : false;

        }
        else
        {
            isGrounded = false;
        }
    }
}
