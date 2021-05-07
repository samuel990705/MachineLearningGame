using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RLAgent : Agent
{
    [SerializeField] private Transform opponentTransform;
    [SerializeField] private Transform ballTransform;

    //set to public so controller can reference these variables
    public int acceleration;
    public int steer;
    public int brake;


    //defines observations available to agent (input of model)
    public override void CollectObservations(VectorSensor sensor)
    {
        //15 inputs total
        sensor.AddObservation(this.transform);//own position (3 inputs)
        sensor.AddObservation(this.transform.rotation);//own rotation (3 inputs)

        sensor.AddObservation(opponentTransform.position);//opponent position (3 inputs)
        sensor.AddObservation(opponentTransform.rotation);//opponent rotation (3 inputs)

        sensor.AddObservation(ballTransform.position);//ball position (3 inputs)
    }


    //defines actions available to agent (output of model)
    public override void OnActionReceived(ActionBuffers actions)
    {
        //actions.DiscreteActions[0]: controls acceleration( 0 is deccelerate, 1 is no acceleration, 2 is accelerate)
        //actions.DiscreteActions[1]: controls steer( 0 is steer left, 1 is forward, 2 is steer right)
        //actions.DiscreteActions[2]: controls steer( 0 is no brake, 1 brake)

        acceleration = actions.DiscreteActions[0];
        steer = actions.DiscreteActions[1];
        brake = actions.DiscreteActions[2];

    }

}
