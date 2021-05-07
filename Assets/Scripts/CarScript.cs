using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    float flipTimer;

    void Start()
    {
        flipTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Dot(transform.up,Vector3.down)>0)//if car is upside down
        {
            flipTimer += Time.deltaTime;
        }
        else//if not flipped
        {
            flipTimer = 0;
        }

        if (flipTimer > 4)//if "flipped" for 4 seconds, flip car upright
        {
            Vector3 newPos = this.transform.position;
            newPos.y += 2f;
            this.transform.position = newPos;

            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);


        }
    }
}
