using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlay : MonoBehaviour
{
    [SerializeField] private Arena arena;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Goal1")) //ball touched Goal1 (Agent 2 scored)
        {
            arena.reset();
        }
        else if (col.gameObject.CompareTag("Goal2")) //ball touched Goal2 (Agent 1 scored)
        {
            arena.reset();
        }
    }
}
