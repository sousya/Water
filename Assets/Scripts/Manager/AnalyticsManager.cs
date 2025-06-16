using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

[MonoSingletonPath("[Analytics]/AnalyticsManager")]
public class AnalyticsManager : MonoSingleton<AnalyticsManager>, ICanGetUtility, ICanSendEvent
{
    private const string ANALYTICS_EVENT_LEVEL_COMPLETE = "level_complete";
    private const string LEVEL = "level";
    private const string DETAILS = "details";

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            //Debug.Log("Unity Services ��ʼ���ɹ�");

            AnalyticsService.Instance.StartDataCollection();
            //Debug.Log("Analytics �����ռ�������");
        }
        catch (System.Exception e)
        {
            //Debug.LogError($"Unity Services ��ʼ��ʧ��: {e.Message}");
            throw;
        }

    }

    public void SendLevelEvent(string del)
    {
        Dictionary<string, object> _levelEvent = new Dictionary<string, object>
        {
            { LEVEL, this.GetUtility<SaveDataUtility>().GetLevelClear()},
            { DETAILS, del}
        };
        SendServerEvent(ANALYTICS_EVENT_LEVEL_COMPLETE, _levelEvent);
    }

    private void SendServerEvent(string eventName, Dictionary<string, object> parameters)
    {
        // ���ݵļ���Ҫ��Unity Analytics��Ԥ�ȶ���
        var customEvent = new CustomEvent(eventName);
        foreach (var pair in parameters)
        {
            customEvent[pair.Key] = pair.Value;
        }

        // �����¼�
        AnalyticsService.Instance.RecordEvent(customEvent);
    }
   
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}