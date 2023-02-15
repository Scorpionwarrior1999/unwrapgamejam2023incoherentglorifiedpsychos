using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCondition : MonoBehaviour
{

    public float timeLeft;


    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <=0)
        {
            Debug.Log("you've lost, time over");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "train")
        {
            Debug.Log("you've lost");
        }
    }
}
