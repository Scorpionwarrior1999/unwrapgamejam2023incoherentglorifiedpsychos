using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField]
    private GameObject _TurnonThisUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "train")
        {
            _TurnonThisUI.SetActive(true);
        }
    }

}
