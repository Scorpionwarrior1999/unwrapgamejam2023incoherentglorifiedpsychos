using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(-transform.right * 1f * Time.deltaTime);
    }
}
