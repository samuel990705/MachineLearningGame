using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    //note car1 is trying to get the ball into goal2, vice versa
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject AI;
    [SerializeField] private GameObject ball;

    private Transform playerTransform;
    private Transform AITransform;
    private Transform ballTransform;

    private Rigidbody playerBody;
    private Rigidbody AIBody;
    private Rigidbody ballBody;

    //used to reset residual force in player and AI
    private bool justReset;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        AITransform = AI.GetComponent<Transform>();
        ballTransform = ball.GetComponent<Transform>();

        playerBody = player.GetComponent<Rigidbody>();
        AIBody = AI.GetComponent<Rigidbody>();
        ballBody = ball.GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        //if the scene was just reset, set rigidbodies back to non-kinematic
        if (justReset)
        {
            justReset = false;
            playerBody.isKinematic = false;
            AIBody.isKinematic = false;
        }
    }

    public void reset()
    {
        //reset transform of player and AI
        playerTransform.position = new Vector3(0f, 2f, 25f);
        playerTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
        AITransform.position = new Vector3(0f, 2f, -25f);
        AITransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        ballTransform.position = new Vector3(0f, 6f, 0f);

        //remove all residual force
        justReset = true;
        playerBody.isKinematic = true;
        AIBody.isKinematic = true;
        ballBody.velocity = Vector3.zero;

    }
}
