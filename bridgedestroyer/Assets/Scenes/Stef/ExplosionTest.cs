using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField] RayfireBomb bomb;
    [SerializeField] RayfireRigid cube;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExplosionButton()
    {
        Debug.Log("Boom");
        cube.physics.useGravity = true;
        bomb.Explode(0);
        cube.Demolish();
    }
}
