using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;

public class RLAgent : Agent
{

    //passed in as observations to the agent
    [SerializeField] private Transform opponentTransform;
    [SerializeField] private Transform ballTransform;

    //set to public so controller can reference these variables
    [HideInInspector] public float acceleration;
    [HideInInspector] public float steer;
    [HideInInspector] public float brake;

    //used to determine which team car is one (team one or team two)
    BehaviorParameters behaviorParameters;
    private Team team;

    public float timePenalty;//sum of penalty for existing (at most sums to 1.0f)
    public float existentialPenalty;//penalty added for existing per action

    //determines team of the car (used determine position cars spawn in)
    public enum Team
    {
        one = 1,
        two = 2
    }

    Vector3 startingPosition;
    Quaternion startingRotation;

    //called once
    public override void Initialize()
    {
        behaviorParameters = gameObject.GetComponent<BehaviorParameters>();//gets BehaviorParameters component
        if (behaviorParameters.TeamId == (int)Team.one)//behaviorParameters.TeamId is a parameter of the component that is preset in the inspector
        {
            team = Team.one;
            startingPosition = new Vector3(0f, 2f, -25f);
            startingRotation= Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            team = Team.two;
            startingPosition = new Vector3(0f, 2f, 25f);
            startingRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        this.MaxStep = 3000;//number of steps taken by agent before environemnt resets (used to speed up training in case it gets stuck somewhere)
        existentialPenalty = 1f / MaxStep;//makes sure timePenalty at most sums to 1
    }

    //an Episode can be thought of as an iteration of training
    public override void OnEpisodeBegin()
    {

        //reset training environment
        timePenalty = 0;
        transform.rotation = startingRotation;
        transform.localPosition = startingPosition;
        ballTransform.localPosition = new Vector3(0f, 5f, 0f);
    }

    //defines observations available to agent (input of model)
    public override void CollectObservations(VectorSensor sensor)
    {
        //15 inputs total
        sensor.AddObservation(this.transform.localPosition);//own position (3 inputs)
        sensor.AddObservation(this.transform.rotation.eulerAngles);//own rotation (3 inputs)

        sensor.AddObservation(opponentTransform.localPosition);//opponent position (3 inputs)
        sensor.AddObservation(opponentTransform.rotation.eulerAngles);//opponent rotation (3 inputs)

        sensor.AddObservation(ballTransform.localPosition);//ball position (3 inputs)
    }


    //defines actions available to agent (output of model)
    public override void OnActionReceived(ActionBuffers actions)
    {
        //actions.DiscreteActions[0]: controls acceleration( -1 is decelerate, 0 is no acceleration, 1 is accelerate)
        //actions.DiscreteActions[1]: controls steer( -1 is steer left, 0 is forward, 1 is steer right)
        //actions.DiscreteActions[2]: controls steer( 0 is no brake, 1 is brake)

        acceleration = actions.DiscreteActions[0]-1;//minus 1 such that values are -1,0,1 (Rather than 0,1,2, which is the default)
        steer = actions.DiscreteActions[1]-1;
        brake = actions.DiscreteActions[2];

        AddReward(-existentialPenalty);//add penalty for existing
        timePenalty += existentialPenalty;//used to deduct from reward when an agent actually scores (this is done in Ball.cs)

    }

    //allows for manual control of AI (for debugging purposes)
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int) Input.GetAxisRaw("Vertical")+1;//can be casted to int since values are always -1,0,1 (no smoothing)
        discreteActions[1] = (int )Input.GetAxisRaw("Horizontal")+1;
        discreteActions[2] = Input.GetKey(KeyCode.Space) == true ? 1 : 0;
    }

    void OnCollisionEnter(Collision col)
    {
        //add reward for touching ball (speeds up training)
        if (col.gameObject.CompareTag("Ball"))
        {
            AddReward(0.1f);
        }

        //discourages colliding with opposing buggy
        if (col.gameObject.CompareTag("Buggy1") || col.gameObject.CompareTag("Buggy2"))//one of these conditions is met if collided with opponent buggy
        {
            SetReward(-0.5f);
            EndEpisode();
        }
    }

}
