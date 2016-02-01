using UnityEngine;
using System.Collections;

public class Schiter : MonoBehaviour
{
    public Camera camera;
    public GameObject flash;

    // Use this for initialization
    void Start()
    {
        camera = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(flash, this.transform.position, this.transform.rotation);
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "otherPlayer")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
            else
            {
                print("I'm looking at nothing!");
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            transform.position = transform.position - new Vector3(-2.0f, 0.0f, 0.0f);
        }
    }
}