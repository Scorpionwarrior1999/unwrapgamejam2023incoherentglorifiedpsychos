using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField] RayfireBomb bomb;
    [SerializeField] RayfireRigid objectToExplode;
    RayfireBomb[] bombList;

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
        if (objectToExplode != null && bomb != null)
        {
            Debug.Log("Boom");
            bomb.Explode(0);
        }
    }
}
