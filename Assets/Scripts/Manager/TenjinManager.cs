using AnyThinkAds.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TenjinManager: MonoBehaviour
{
    static public TenjinManager Instance;
    public TopOnCtrl topOnCtrl;
    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        //Debug.Log("TenjinTime " + Time.time);
        //Debug.Log("Tenjin StartConnect0");

        BaseTenjin instance = Tenjin.getInstance("CMPSNGWPF6EXGKL1JS1DAMRVF7OMD2LS");
        //Debug.Log("Tenjin StartConnect1");

#if UNITY_ANDROID
        //Debug.Log("Tenjin StartConnect2");

        instance.SetAppStoreType(AppStoreType.googleplay);
        // Sends install/open event to Tenjin
        instance.Connect();
#endif
        StartCoroutine(initTopOn());
    }

    IEnumerator initTopOn()
    {
        yield return new WaitForSeconds(0.2f);
        topOnCtrl.Init();

    }

    void OnApplicationPause(bool pauseStatus)
    {
        //if (!pauseStatus)
        //{
        //    TenjinConnect();
        //}
    }

    public void TenjinConnect()
    {

    }

}
