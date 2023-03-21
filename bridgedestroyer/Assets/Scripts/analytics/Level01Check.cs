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
    [SerializeField]
    private string _string;
    private bool _hasDoneThing = false;
    void Start()
    {
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, _string) ;
        
    }

    
    void Update()
    {
        if (!_hasDoneThing)
        {

            if (_winScreen.active)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, _string);
                _hasDoneThing = true;
            }
            else if (_lossscreen.active)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, _string);
                _hasDoneThing = true;
            }
        }
           
    }
}
