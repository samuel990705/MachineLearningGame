using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	private float horizontalInput;//A,D
	private float verticalInput;//W,S
	private float steerAngle;//current angle being steered

	//referene to respective wheel colliders
	public WheelCollider LFWheel, RFWheel;//left and right front wheel
	public WheelCollider LRWheel, RRWheel;//left and right rear wheel

	//transform of wheels
	public Transform LFTransform, RFTransform;
	public Transform LRTransform, RRTransform;

	public float maxSteerAngle = 35;//limits how fast car can steer
	public float motorForce = 1000;//force applied when accelerating
	public float brakeTorque = 500;//force applied when accelerating
	public float driftingStiffness = 0.5f;//stiffness of rear wheels when brake is held (to allow for drifting)
	public float defaultStiffness = 1.75f;//stiffness of rear wheels when brake is not held (so that car doesn't slide around)


	private void FixedUpdate()
    {
		//get input
		horizontalInput = Input.GetAxisRaw("Horizontal");//GetAxisRaw is used to eliminate smoothing so that AI Agent's decision is discrete
		verticalInput = Input.GetAxisRaw("Vertical");

		//update current steer angle
		steerAngle = maxSteerAngle * horizontalInput;
		LFWheel.steerAngle = this.steerAngle;
		RFWheel.steerAngle = this.steerAngle;

		//update acceleration (only powers front wheels)
		LFWheel.motorTorque = motorForce * verticalInput;
		RFWheel.motorTorque = motorForce * verticalInput;
		//LRWheel.motorTorque = motorForce * verticalInput;
		//RRWheel.motorTorque = motorForce * verticalInput;

		//if brake key is held
		float brake = Input.GetKey(KeyCode.Space) == true ? brakeTorque : 0;
		LRWheel.brakeTorque = brake;//add brakeTorque to rear wheels
		RRWheel.brakeTorque = brake;

		float stiffness = Input.GetKey(KeyCode.Space) == true ? driftingStiffness : defaultStiffness;//1.0f is default stiffness of wheels
		WheelFrictionCurve sidewaysFriction = LRWheel.sidewaysFriction;
		sidewaysFriction.stiffness = stiffness;
		LRWheel.sidewaysFriction = sidewaysFriction;//change stiffness of rear wheels
		RRWheel.sidewaysFriction = sidewaysFriction;

		//update rotation and position of wheels
		updateWheelTransform(LFWheel, LFTransform);
		updateWheelTransform(RFWheel, RFTransform);
		updateWheelTransform(LRWheel, LRTransform);
		updateWheelTransform(RRWheel, RRTransform);


	}

	//update the rotation and transform of the wheel
	private void updateWheelTransform(WheelCollider wheelCollider , Transform wheelTransform)
    {
		Vector3 pos = wheelTransform.position;
		Quaternion rot = wheelTransform.rotation;

		wheelCollider.GetWorldPose(out pos, out rot);

		wheelTransform.position = pos;
		wheelTransform.rotation = rot;

    }
}
