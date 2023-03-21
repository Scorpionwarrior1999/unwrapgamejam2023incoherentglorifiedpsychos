using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class Level01Check : MonoBehaviour
{
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private GameObject _lossscreen;
    void Start()
    {
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level01") ;
    }

    
    void Update()
    {
        if(_winScreen.active)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level01");
        }
        else if (_lossscreen.active)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level01");
        }
           
    }
}
