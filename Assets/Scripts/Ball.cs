using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private RLAgent agent1;//RLAgent script of car1
    [SerializeField] private RLAgent agent2;

    public float scoreReward=1f;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Goal1")) //ball touched Goal1 (Agent 2 scored)
        {
            agent2.AddReward(scoreReward);//deducts penalty depending on how long it took
        }else if (col.gameObject.CompareTag("Goal2")) //ball touched Goal2 (Agent 1 scored)
        {
            agent1.AddReward(scoreReward);
        }
        else
        {
            return;//dont end episode unless collided with goal
        }


        //episode ends once someone scores
        agent1.EndEpisode();
        agent2.EndEpisode();
    }
}
