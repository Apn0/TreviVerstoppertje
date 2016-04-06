using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AnimationApplier : NetworkBehaviour
{

    public float groundCheckDistance;
    public float abovestart;

    private Transform myCamera;
    private Transform neck;
    private Transform spine;
    private Transform leftArm;
    private Transform rightArm;

    public Vector3 headBoneRot;
    public float myXpos;

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

        spine = transform.Find("Graphics/mixamorig:Hips/mixamorig:Spine");
        neck = transform.Find("Graphics/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck");
       // leftArm = transform.Find("Graphics/Hips/Spine/Spine1/Spine2/LeftShoulder");
       // rightArm = transform.Find("Graphics/Hips/Spine/Spine1/Spine2/RightShoulder");

        leftArm = transform.Find("Graphics/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm");
        rightArm = transform.Find("Graphics/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm");
        myCamera = transform.Find("Camera");
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
        if (myCamera.localEulerAngles.x > 180)
        {
            myXpos = myCamera.localEulerAngles.x - 360;
        }
        else
        {
            myXpos = myCamera.localEulerAngles.x;
        }
        spine.localEulerAngles += new Vector3(myXpos * .25f, 0, 0);
        neck.localEulerAngles += new Vector3(myXpos * 0.75f, 0, 0);

       // leftArm.eulerAngles += new Vector3(myXpos * 0.75f, 0, 0);
        rightArm.eulerAngles += new Vector3(myXpos * 0.75f, 0, 0);

        //1. verander waarde heupen. 
        //Crouch 17 graden.


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
