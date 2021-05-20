using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBall : MonoBehaviour
{
    [SerializeField] private RLCube cube1;
    [SerializeField] private RLCube cube2;

    public float scoreReward = 1f;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Goal1")) //ball touched Goal1 (Agent 2 scored)
        {
            cube2.AddReward(scoreReward);//deducts penalty depending on how long it took
        }
        else if (col.gameObject.CompareTag("Goal2")) //ball touched Goal2 (Agent 1 scored)
        {
            cube1.AddReward(scoreReward);
        }
        else
        {
            return;//dont end episode unless collided with goal
        }


        //episode ends once someone scores
        cube1.EndEpisode();
        cube2.EndEpisode();
    }
}
