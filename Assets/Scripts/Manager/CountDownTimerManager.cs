using UnityEngine;
using System;
using System.Collections.Generic;
using QFramework;
using System.Globalization;

public class CountDownTimerManager : MonoSingleton<CountDownTimerManager>
{
    private static readonly string COUNTDOWN_TIMER_SIGN = "CountDownTimer_";
    private readonly Dictionary<string, CountDownTimer> timers = new();

    public class CountDownTimer
    {
        private DateTime EndTime;
        public bool IsFinished { get; private set; }

        public CountDownTimer(DateTime endTime)
        {
            EndTime = endTime;
            IsFinished = false;
        }

        public TimeSpan GetRemainingTime()
        {
            return EndTime - DateTime.UtcNow;
        }

        public bool Update()
        {
            if (IsFinished) return false;

            var remaining = GetRemainingTime();
            if (remaining <= TimeSpan.Zero)
            {
                IsFinished = true;
                return false;
            }
            return true;
        }

        public void Reset(string id, DateTime newEndTime)
        {
            EndTime = newEndTime;
            IsFinished = false;

            string key = COUNTDOWN_TIMER_SIGN + id;
            PlayerPrefs.SetString(key, EndTime.ToString("o"));
            PlayerPrefs.Save();
        }
    }

    public override void OnSingletonInit()
    {
       
    }

    //������ʱ��
    public void StartTimer(string id, double minutes)
    {
        //TimeSpan duration = TimeSpan.FromHours(hours);
        TimeSpan duration = TimeSpan.FromMinutes(minutes);
        timers[id] = CreateFromPrefs(id, duration);
    }

    //���ü�ʱ��
    public void ResetTimer(string id, double minutes)
    {
        //TimeSpan duration = TimeSpan.FromHours(hours);
        TimeSpan duration = TimeSpan.FromMinutes(minutes);
        string key = COUNTDOWN_TIMER_SIGN + id;

        if (PlayerPrefs.HasKey(key))
            PlayerPrefs.DeleteKey(key);

        timers[id] = CreateFromPrefs(id, duration);
    }

    //��ʱ�����ʱ��
    public void AddTimer(string id, double minutes)
    {
        string key = COUNTDOWN_TIMER_SIGN + id;
        TimeSpan duration = TimeSpan.FromMinutes(minutes);
        //�޼�¼����
        if (!PlayerPrefs.HasKey(key))
            timers[id] = CreateFromPrefs(id, duration);
        else
        {
            if (DateTime.TryParse(PlayerPrefs.GetString(key), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var endTime))
            {
                //�������¼�ʱ
                if ((endTime - DateTime.UtcNow) <= TimeSpan.Zero)
                {
                    ResetTimer(id, minutes);
                }
                //δ��������ʱ��
                else
                {
                    var remaining = endTime - DateTime.UtcNow;
                    var newMinutes = remaining.TotalMinutes + minutes;
                    ResetTimer(id, newMinutes);
                }
            }
        }
    }

    //��ȡ��ʱ���Ƿ����
    public bool IsTimerFinished(string id)
    {
        string key = COUNTDOWN_TIMER_SIGN + id;
        if (PlayerPrefs.HasKey(key))
        {
            if (DateTime.TryParse(PlayerPrefs.GetString(key), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var endTime))
            {
                return (endTime - DateTime.UtcNow) <= TimeSpan.Zero;
            }
        }
        
        return true;
    }

    //��ȡʣ��ʱ���ı�
    public string GetRemainingTimeText(string id)
    {
        string key = COUNTDOWN_TIMER_SIGN + id;

        if (PlayerPrefs.HasKey(key))
        {
            if (DateTime.TryParse(PlayerPrefs.GetString(key), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var endTime))
            {
                var remaining = endTime - DateTime.UtcNow;

                if (remaining < TimeSpan.Zero)
                    remaining = TimeSpan.Zero;

                return string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)remaining.TotalHours,
                    remaining.Minutes,
                    remaining.Seconds);
            }
        }

        return "00:00:00";
    }

    //��������
    private CountDownTimer CreateFromPrefs(string id, TimeSpan duration)
    {
        string key = COUNTDOWN_TIMER_SIGN + id;
        DateTime endTime;
        bool needSave = false;

        if (PlayerPrefs.HasKey(key))
        {
            var str = PlayerPrefs.GetString(key);
            if (!DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out endTime))
            {
                endTime = DateTime.UtcNow + duration;
                needSave = true;
            }
        }
        else
        {
            endTime = DateTime.UtcNow + duration;
            needSave = true;
        }

        if (needSave)
        {
            PlayerPrefs.SetString(key, endTime.ToString("o"));
            PlayerPrefs.Save();
        }

        return new CountDownTimer(endTime);
    }

    /*private void Update()
    {
        var removeKeys = new List<string>();
        foreach (var kv in timers)
        {
            var id = kv.Key;
            var timer = kv.Value;

            if (!timer.Update())
            {
                removeKeys.Add(id);
            }
        }

        // �Ƴ������ļ�ʱ��
        foreach (var id in removeKeys)
        {
            timers.Remove(id);
        }
    }*/
}
