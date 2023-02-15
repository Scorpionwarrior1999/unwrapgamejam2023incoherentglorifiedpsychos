using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hingeScript : MonoBehaviour
{
    private List<HingeJoint> _joints = new List<HingeJoint>();
    void Start()
    {
        _joints.AddRange(gameObject.GetComponents<HingeJoint>());
    }


    void Update()
    {
        //foreach(HingeJoint j in _joints)
        //{
        //    if(j.connectedBody == null)
        //    {
        //        _joints.Remove(j);

        //        Destroy(j);
        //    }
        //}
        for (int i = _joints.Count - 1; i > -1; i--)
        {
            if (_joints[i].connectedBody == null)
            {
                HingeJoint p = _joints[i];
                _joints.Remove(_joints[i]);
                Destroy(p);
                p = null;
            }
        }

    }
}
