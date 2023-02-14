using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moneyhandler : MonoBehaviour
{
    public int money;
    public int dynamiteCost;
    public bool dynamitePlaced;
    public bool dynamiteRemoved;
    [SerializeField]
    private Text moneyText;

    public int smallCost;
    public int midCost;
    public int bigCost;

    private void Start()
    {
        dynamitePlaced = false;
    }

    private void Update()
    {
        moneyText.text = money.ToString();

        if (dynamitePlaced)
        {
            if ((money - dynamiteCost) >= 0)
            {
                money -= dynamiteCost;
            }
            else
            {

            }
            dynamitePlaced = false;

        }
        if (dynamiteRemoved)
        {
            money += dynamiteCost;
            dynamiteRemoved = false;
        }
        if (money < 0)
        {
            money = 0;
        }
    }
}
