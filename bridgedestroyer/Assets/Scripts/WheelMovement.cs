using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;

    private float _angle;

    void Update()
    {
        _angle += Time.deltaTime * _rotationSpeed;
        transform.localRotation = Quaternion.Euler(new Vector3(_angle, 0, 0));
    }
}
