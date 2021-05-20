using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    CharacterController controller;
    float moveSpeed = 30.0f;
    float rotateSpeed = 100.0f;
    float gravity = 25.0f;
    Vector3 moveDirection;
    float xMove;
    float yRotate;

    RLCube cube;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = new Vector3();
        xMove = 0f;
        yRotate = 0f;

        cube = gameObject.GetComponent<RLCube>();
    }

    private void FixedUpdate()
    {
        //moveDirection = new Vector3(0f, 0f, xMove);
        moveDirection = transform.TransformDirection(Vector3.forward) * xMove;
        moveDirection *= moveSpeed;

        transform.Rotate(new Vector3(0f, yRotate * rotateSpeed * Time.fixedDeltaTime, 0f));

        moveDirection.y -= gravity;



        controller.Move(moveDirection * Time.fixedDeltaTime);
    }

    void Update()
    {
        xMove = cube.verticalInput;
        yRotate = cube.horizontalInput;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            Vector3 direction = body.transform.localPosition - transform.localPosition;
            body.AddForceAtPosition(direction.normalized, transform.localPosition);
        }
    }
}
