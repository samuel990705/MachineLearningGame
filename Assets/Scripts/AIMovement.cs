using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
	//same as player input but for AI
	public float horizontalInput;
	public float verticalInput;
	public float steerAngle;//current angle being steered

	//referene to respective wheel colliders
	public WheelCollider LFWheel, RFWheel;//left and right front wheel
	public WheelCollider LRWheel, RRWheel;//left and right rear wheel

	//transform of wheels
	public Transform LFTransform, RFTransform;
	public Transform LRTransform, RRTransform;

	public float maxSteerAngle = 35;//limits how fast car can steer
	public float motorForce = 1000;//force applied when accelerating
	public float brakeTorque = 500;//force applied when accelerating
	public float driftingStiffness = 0.75f;//stiffness of rear wheels when brake is held (to allow for drifting)
	public float defaultStiffness = 1.75f;//stiffness of rear wheels when brake is not held (so that car doesn't slide around)

	RLAgent agent;//reference to the RLAgent script

    private void Start()
    {
		agent = gameObject.GetComponent<RLAgent>(); 
	}

    private void FixedUpdate()
	{
		//fetch input from agent
		verticalInput = agent.acceleration;//(-1 because acceleration is either 0,1,2)
		horizontalInput = agent.steer;//(-1 because steer is either 0,1,2)


		//update current steer angle
		steerAngle = maxSteerAngle * horizontalInput;
		LFWheel.steerAngle = this.steerAngle;
		RFWheel.steerAngle = this.steerAngle;

		//update acceleration (Power all 4 wheels: All-wheel drive)
		LFWheel.motorTorque = motorForce * verticalInput;
		RFWheel.motorTorque = motorForce * verticalInput;
		LRWheel.motorTorque = motorForce * verticalInput;
		RRWheel.motorTorque = motorForce * verticalInput;

		//if brake key is held
		//float brake = agent.brake == 1 ? brakeTorque : 0;//if agent.brake==1, then brake, otherwise dont brake
		//LRWheel.brakeTorque = brake;//add brakeTorque to rear wheels
		//RRWheel.brakeTorque = brake;

		//float stiffness = brake = agent.brake == 1 ? driftingStiffness : defaultStiffness;//1.0f is default stiffness of wheels
		//WheelFrictionCurve sidewaysFriction = LRWheel.sidewaysFriction;
		//sidewaysFriction.stiffness = stiffness;
		//LRWheel.sidewaysFriction = sidewaysFriction;//change stiffness of rear wheels
		//RRWheel.sidewaysFriction = sidewaysFriction;

		//update rotation and position of wheels
		updateWheelTransform(LFWheel, LFTransform);
		updateWheelTransform(RFWheel, RFTransform);
		updateWheelTransform(LRWheel, LRTransform);
		updateWheelTransform(RRWheel, RRTransform);
	}

	//update the rotation and transform of the wheel
	private void updateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
	{
		Vector3 pos = wheelTransform.position;
		Quaternion rot = wheelTransform.rotation;

		wheelCollider.GetWorldPose(out pos, out rot);

		wheelTransform.position = pos;
		wheelTransform.rotation = rot;

	}
}

