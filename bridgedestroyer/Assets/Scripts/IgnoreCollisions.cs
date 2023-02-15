using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    [SerializeField] private Collider collider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hinge" || collision.gameObject.tag == "WoodStruct")
        {
            Physics.IgnoreCollision(collision.collider, collider);
        }
    }
}
