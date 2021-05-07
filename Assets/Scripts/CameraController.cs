using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform car;
    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;

    private void FixedUpdate()
    {
        //look at target
        Vector3 lookDirection = car.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);//for smooth movement of camera

        //move to target

        Vector3 targetPosition = car.position//calculate new camera position
            + car.forward * offset.z
            + car.right * offset.x
            + car.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
