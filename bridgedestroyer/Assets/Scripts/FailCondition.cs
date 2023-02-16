using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCondition : MonoBehaviour
{

    public float timeLeft;
    [SerializeField]
    private GameObject defeatScreen;
    [SerializeField]
    private GameObject _winScreen;


    private void Start()
    {
        defeatScreen.SetActive(false);
    }

    private void Update()
    {
        if (!_winScreen.active)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                Debug.Log("you've lost, time over");
                defeatScreen.SetActive(true);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "train")
        {
            Debug.Log("you've lost");
            defeatScreen.SetActive(true);
        }
    }
}
