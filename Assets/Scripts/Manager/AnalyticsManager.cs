using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

[MonoSingletonPath("[Analytics]/AnalyticsManager")]
public class AnalyticsManager : MonoSingleton<AnalyticsManager>, ICanGetUtility, ICanSendEvent
{
    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            //Debug.Log("Unity Services 初始化成功");

            AnalyticsService.Instance.StartDataCollection();
            //Debug.Log("Analytics 数据收集已启动");

            TestSendEvent();
        }
        catch (System.Exception e)
        {
            //Debug.LogError($"Unity Services 初始化失败: {e.Message}");
            throw;
        }

    }

    public void SendServerEvent(string eventName, Dictionary<string, object> parameters)
    {
        // 旧版-已弃用
        //AnalyticsService.Instance.CustomData(eventName, parameters);

        // 新版-发送自定义事件方法 示例
        var customEvent = new CustomEvent("level_complete");
        customEvent["level"] = 5;
        customEvent["time_spent"] = 120.5f;

        // 发送事件
        AnalyticsService.Instance.RecordEvent(customEvent);
    }

    public void TestSendEvent()
    {
        //传递的键需要在Unity Analytics中预先定义
        var customEvent = new CustomEvent("completeLevel");
        customEvent["level"] = 5;
        customEvent["details"] = "自定义测试事件";

        // 发送事件
        AnalyticsService.Instance.RecordEvent(customEvent);
        //Debug.Log("发送自定义数据分析事件");
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}