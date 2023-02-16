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
    private AudioSource _explosionSound;
    private void Awake()
    {
        Activate = false;
        _explosionSound = GetComponent<AudioSource>();
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
                    ApplyForceTOStuff(1);

                }
                else if (_bombType == BomType.Medium)
                {
                    DestroyStuff(3);
                    ApplyForceTOStuff(3);
                }
                else if (_bombType == BomType.Big)
                {
                    DestroyStuff(6);
                    ApplyForceTOStuff(6);
                }
            }
        }
    }

    private void DestroyStuff(float range)
    {
        _explosionSound.Play(0);

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
    private void ApplyForceTOStuff(float range)
    {
        Collider[] stuff = Physics.OverlapSphere(transform.position, range);
        List<Rigidbody> rigs = new List<Rigidbody>();

        foreach (Collider c in stuff)
        {
            if (c.gameObject.TryGetComponent<Rigidbody>(out Rigidbody r))
            {
                rigs.Add(r);
            }
        }
        foreach (Rigidbody ri in rigs)
        {
            Vector3 dir = (ri.transform.position - transform.position);
            dir = dir.normalized;
            dir = dir * range;
            ri.AddForce( dir ,ForceMode.Impulse);
        }
    }
}
