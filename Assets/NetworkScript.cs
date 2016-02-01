using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour {
    public GameObject cam;
    private bool previouslyGrounded;
   // [SerializeField]
   // public bool grounded;
    CharacterController cc;
    internal static bool grounded;

    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        if (!isLocalPlayer)
        {
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            cam.SetActive(false);
        }        
    }
   /** void Update()
    {
        if(cc.isGrounded && !previouslyGrounded)
        {
            Call server for grounded!
          //  CmdGrounded(true);
            previouslyGrounded = true;
        }
        if(!cc.isGrounded && previouslyGrounded)
        {
            CmdGrounded(false);
            previouslyGrounded = false;
        }
    }

    [Command]
    void CmdGrounded(bool bol)
    {
        grounded = bol;

    }
	
	// Update is called once per frame
**/}
