using AnyThinkAds.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopOnCtrl: MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Init()
    {

        Debug.Log("TopOnTime " + Time.time);
        ATSDKAPI.initSDK("a66b2e6a0c5e90", "a4b878aec33a279a29e5027f1abb23ca1");
        ATSDKAPI.setLogDebug(false);
        TopOnADManager.Instance.LoadAD();
        DontDestroyOnLoad(gameObject);
    }


}
