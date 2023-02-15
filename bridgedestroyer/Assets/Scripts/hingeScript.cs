using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hingeScript : MonoBehaviour
{
    private List<HingeJoint> _joints = new List<HingeJoint>();




    void Update()
    {


        _joints.AddRange(gameObject.GetComponents<HingeJoint>());
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
        _joints.Clear();



    }
}
