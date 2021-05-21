using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;

public class RLCube : Agent
{

    [SerializeField] private GameObject opponent;
    private Transform opponentTransform;
    private RLCube opponentAgent;


    [SerializeField] private GameObject ball;
    private Transform ballTransform;
    private Rigidbody ballBody;

    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;

    BehaviorParameters behaviorParameters;
    private Team team;
    public enum Team
    {
        one = 1,
        two = 2
    }

    public float timePenalty;
    public float existentialPenalty;

    Vector3 startingPosition;
    Quaternion startingRotation;

    private Rigidbody body;

    public override void Initialize()
    {
        ballTransform=ball.GetComponent<Transform>();
        ballBody = ball.GetComponent<Rigidbody>();
        body = gameObject.GetComponent<Rigidbody>();
        opponentTransform=opponent.GetComponent<Transform>();
        opponentAgent = opponent.GetComponent<RLCube>();
        behaviorParameters = gameObject.GetComponent<BehaviorParameters>();//gets BehaviorParameters component
        if (behaviorParameters.TeamId == (int)Team.one)//behaviorParameters.TeamId is a parameter of the component that is preset in the inspector
        {
            team = Team.one;
            startingPosition = new Vector3(0f, 2.2f, -25f);
            startingRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            team = Team.two;
            startingPosition = new Vector3(0f, 2.2f, 25f);
            startingRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        this.MaxStep = 3000;//number of steps taken by agent before environemnt resets (used to speed up training in case it gets stuck somewhere)
        existentialPenalty = 1f / MaxStep;//makes sure timePenalty at most sums to 1
    }

    public override void OnEpisodeBegin()
    {
        timePenalty = 0;

        transform.localPosition = startingPosition + new Vector3(Random.Range(-18f, 18f),0f, Random.Range(-15f, 15f));
        body.velocity = Vector3.zero;
        ballBody.velocity = Vector3.zero;
        ballTransform.localPosition = new Vector3(0f, 5f, 0f);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //6 inputs total
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);

        sensor.AddObservation(opponentTransform.localPosition.x);
        sensor.AddObservation(opponentTransform.localPosition.z);


        sensor.AddObservation(ballTransform.localPosition.x);
        sensor.AddObservation(ballTransform.localPosition.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //actions.DiscreteActions[0]: controls vertical movement( -1 is backward, 0 is stay, 1 is forward)
        //actions.DiscreteActions[1]: controls horizontal movement( -1 is left, 0 is stay, 1 is right)
        verticalInput = actions.DiscreteActions[0] - 1;
        horizontalInput = actions.DiscreteActions[1] - 1;

        //Debug.Log(verticalInput + ", " + horizontalInput);

        AddReward(-existentialPenalty);
        timePenalty += existentialPenalty;//used to deduct from reward when an agent actually scores (this is done in Ball.cs)
    }

    //used to manually control cube when there is no model
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxisRaw("Vertical") + 1;
        discreteActions[1] = (int)Input.GetAxisRaw("Horizontal") + 1;
    }

    void OnCollisionEnter(Collision col)
    {
        //add reward for touching ball (speeds up training)
        if (col.gameObject.CompareTag("Ball"))
        {
            //AddReward(0.15f);
            //AddReward(1.0f);
            //opponentAgent.EndEpisode();
            //EndEpisode();
            //Debug.Log(team + "Touched Ball");
        }

        //discourages colliding with opponent
        if (col.gameObject.CompareTag("Cube1") || col.gameObject.CompareTag("Cube2"))//one of these conditions is met if collided with opponent buggy
        {
            AddReward(-0.025f);
            //Debug.Log("player collide");
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            //SetReward(-1.0f);
            //opponentAgent.EndEpisode();
            //EndEpisode();
        }
    }
}
