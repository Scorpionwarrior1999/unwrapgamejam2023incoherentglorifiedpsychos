using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool Activate = false;
    [SerializeField]
    private LayerMask _hinges;
    [SerializeField]
    private BomType _bombType;
    private bool _hasBoom = false;
    private void Awake()
    {
        Activate = false;
    }
    void Update()
    {
        if (!_hasBoom)
        {
            if (Activate)
            {
                if (_bombType == BomType.Small)
                {
                    DestroyStuff(1);

                }
                else if (_bombType == BomType.Medium)
                {
                    DestroyStuff(5);
                }
                else if (_bombType == BomType.Big)
                {
                    DestroyStuff(10);
                }
            }
        }
    }

    private void DestroyStuff(float range)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, range, _hinges);
        for (int i = col.Length - 1; i > -1; i--)
        {
            for (int j = col[i].gameObject.GetComponents<HingeJoint>().Length - 1; j > -1; j--)
            {
                HingeJoint[] hinges = col[i].gameObject.GetComponents<HingeJoint>();
                Destroy(hinges[j]);
            }
        }
        Destroy(gameObject);
        _hasBoom = true;
    }
}
