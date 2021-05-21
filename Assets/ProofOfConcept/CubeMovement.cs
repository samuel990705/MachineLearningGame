using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    //CharacterController controller;
    Rigidbody body;
    float moveSpeed = 0.45f;
    float rotateSpeed = 100.0f;
    float gravity = 25.0f;
    Vector3 moveDirection;
    float xMove;
    float yMove;
    //float yRotate;

    float ballForce = 2000f;

    [SerializeField] private GameObject ball;
    Rigidbody ballBody;

    RLCube cube;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        moveDirection = new Vector3();
        xMove = 0f;
        yMove = 0f;
        //yRotate = 0f;

        cube = gameObject.GetComponent<RLCube>();
        ballBody=ball.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //moveDirection = transform.TransformDirection(Vector3.forward) * xMove;
        //moveDirection *= moveSpeed;
        //transform.Rotate(new Vector3(0f, yRotate * rotateSpeed * Time.fixedDeltaTime, 0f));
        //moveDirection.y -= gravity;
        

        moveDirection = new Vector3(yMove, 0.0f, xMove);
        moveDirection *= moveSpeed;
        //moveDirection.y -= gravity;
        body.MovePosition(transform.position + moveDirection);
        //controller.Move(moveDirection * Time.fixedDeltaTime);
    }

    void Update()
    {
        xMove = cube.verticalInput;
        yMove = cube.horizontalInput;
        //yRotate = cube.horizontalInput;
    }

    void OnCollisionEnter(Collision hit)
    {

        //kick ball
        if (hit.gameObject.CompareTag("Ball"))
        {
            Vector3 dir = hit.contacts[0].point - transform.position;
            dir = dir.normalized;
            ballBody.AddForce(dir * ballForce);
        }
    }
}
