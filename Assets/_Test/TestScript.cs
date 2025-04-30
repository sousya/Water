using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Button rebtn;
    public Button addbtn;

    private void Awake()
    {
        HealthManager healthManager = HealthManager.Instance;
    }

    void Start()
    {
        rebtn.onClick.AddListener(() =>
        {
            HealthManager.Instance.CancelUnLimitHp();
        });

        addbtn.onClick.AddListener(() =>
        {
            HealthManager.Instance.SetUnLimitHp(120);
        });
        //Debug.Log(IsNetworkReachability());
    }

    /// <summary>
    /// 网络可达性
    /// </summary> 
    /// <returns></returns>
    public bool IsNetworkReachability()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                print("当前使用的是：WiFi，请放心更新！");
                return true;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                print("当前使用的是移动网络，是否继续更新？");
                return true;
            default:
                print("当前没有联网，请您先联网后再进行操作！");
                return false;
        }
    }    
}
