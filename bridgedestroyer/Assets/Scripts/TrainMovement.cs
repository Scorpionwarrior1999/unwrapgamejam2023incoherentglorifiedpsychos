using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    [SerializeField]
    private float trainSpeed;

    void Update()
    {
        transform.Translate(-transform.right * trainSpeed * Time.deltaTime);
    }
}
