using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDynamite : MonoBehaviour
{
    [SerializeField]
    private Moneyhandler moneyHandler;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "SmallBomb")
                {
                    Destroy(hit.transform.parent.gameObject);
                    moneyHandler.dynamiteCost = moneyHandler.smallCost;
                    moneyHandler.dynamiteRemoved = true;
                }
                else if (hit.transform.tag == "MediumBomb")
                {
                    Destroy(hit.transform.parent.gameObject);
                    moneyHandler.dynamiteCost = moneyHandler.midCost;
                    moneyHandler.dynamiteRemoved = true;
                }
                else if (hit.transform.tag == "BigBomb")
                {
                    Destroy(hit.transform.parent.gameObject);
                    moneyHandler.dynamiteCost = moneyHandler.bigCost;
                    moneyHandler.dynamiteRemoved = true;
                }
            }
        }
    }
}
