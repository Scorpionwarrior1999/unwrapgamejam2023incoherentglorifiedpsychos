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
    [SerializeField]
    private Text smallText;
    public int midCost;
    [SerializeField]
    private Text midText;
    public int bigCost;
    [SerializeField]
    private Text bigText;

    private void Start()
    {
        dynamitePlaced = false;
        smallText.text = smallCost.ToString();
        midText.text = midCost.ToString();
        bigText.text = bigCost.ToString();
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
