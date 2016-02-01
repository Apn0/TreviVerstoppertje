using UnityEngine;
using System.Collections;

public class AnimationApplier : MonoBehaviour {
    public float velocity;
    private Vector3 current;
    private Vector3 previous;
    public bool isIdle, isRunning, isWalking;

    // Use this for initialization
    void Start()
    {
    }
    void Update()
    {
        velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;

        Debug.Log(velocity);
        if (velocity <= 0.1f)
        {
            isIdle = true;
            isRunning = false;
            isWalking = false;
        }
        else if(velocity > 10f)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
        }
        else
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
        }
    }
}
