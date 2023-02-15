using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "train")
        {
            Debug.Log("you've lost");
        }
    }
}
