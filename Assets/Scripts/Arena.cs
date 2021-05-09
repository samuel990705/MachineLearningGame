using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    //note car1 is trying to get the ball into goal2, vice versa
    [SerializeField] private GameObject car1;
    [SerializeField] private GameObject car2;

    private Transform car1Transform;
    private Transform car2Transform;

    private RLAgent car1Agent;
    private RLAgent car2Agent;



    [SerializeField] private GameObject goal1;
    [SerializeField] private GameObject goal2;

    void Start()
    {
        car1Transform = car1.GetComponent<Transform>();
        car2Transform = car2.GetComponent<Transform>();
        car1Agent = car1.GetComponent<RLAgent>();
        car2Agent = car2.GetComponent<RLAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
