using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[MonoSingletonPath("[Health]/HealthManager")]
public class HealthManager : MonoSingleton<HealthManager> ,ICanSendEvent
{
    private const int MAXHP = 5;
    private const float RECOVERTIME = 1800;
    [SerializeField] private int nowHp;
    //体力完全恢复的时间点
    private DateTime recoverEndTime;
    [SerializeField] private string recoverTimeStr;

    //用于后续无限体力功能
    public bool UseHpSign = true;

    /// <summary>
    /// 当前体力恢复剩余时间
    /// </summary>
    public string RecoverTimerStr => recoverTimeStr;

    /// <summary>
    /// 当前体力
    /// </summary>
    public int NowHp => nowHp;

    /// <summary>
    /// 已使用体力
    /// </summary>
    public int UsedHp => MAXHP - nowHp;

    /// <summary>
    /// 当前恢复的体力点
    /// </summary>
    public int CurRecoverySlot => nowHp == MAXHP ? MAXHP : nowHp + 1;

    /// <summary>
    /// 是否有体力
    /// </summary>
    public bool HasHp => nowHp > 0;

    #region QF

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public override void OnSingletonInit()
    {
        Init();
    }

    #endregion

    #region Private Function

    private void Init()
    {
        nowHp = GetNowHp();
        recoverTimeStr = "00:00";
        string time = GetRecoverEndTime();

        if (!string.IsNullOrEmpty(time))
            recoverEndTime = DateTime.Parse(time);
        else
            recoverEndTime = DateTime.MinValue;

        //检查体力是否需要恢复
        CheckRecoverHp();
    }

    /// <summary>
    /// 检查体力是否需要恢复
    /// </summary>
    private void CheckRecoverHp()
    {
        //默认值无消耗体力
        if (recoverEndTime == DateTime.MinValue)
            return;

        float timer = GetrecoverTime();
        // 体力恢复完成
        if (timer <= 0)
            SaveNowHpToMax();
        else
        {
            //计算还需要恢复多少点体力 ， 回满所需时间 / 一点体力恢复时间  =》 向上取整
            int num = (int)Math.Ceiling(timer / RECOVERTIME);
            //当回满所需值 num大于或者等于5时(理论不会出现大于maxHp情况) ，代表体力一定是0点
            nowHp = num >= MAXHP ? 0 : MAXHP - num;
            SaveNowHp(nowHp);
        }
    }

    /// <summary>
    /// 获取恢复所有体力的所需时间
    /// </summary>
    /// <returns></returns>
    private float GetrecoverTime()
    {
        //获取当前时间与 恢复完成时间的时间间隔
        TimeSpan recoverInterval = recoverEndTime - DateTime.Now;
        float remainingTime = (float)recoverInterval.TotalSeconds;
        return remainingTime;
    }

    /// <summary>
    /// 获取当前这一点体力的恢复时间
    /// </summary>
    /// <returns></returns>
    private TimeSpan GetNowRecoverTime()
    {
        // 用于计算当前体力对应的恢复时间(即往前倒推 hpDifference 个恢复周期)
        int hpDifference = MAXHP - nowHp - 1;
        // AddSeconds,增加秒数(扣掉hpDifference点恢复时间，剩余的表示当前体力恢复时长)
        DateTime nowRecoverTime = recoverEndTime.AddSeconds(-RECOVERTIME * hpDifference);
        // 获取当前时间与恢复完成时间的时间间隔
        TimeSpan recoverInterval = nowRecoverTime - DateTime.Now;
        if (recoverInterval.TotalSeconds < 0)
        {
            recoverInterval = TimeSpan.Zero;
        }
        return recoverInterval;
    }

    private void Update()
    {
        if (nowHp < MAXHP)
        {
            //TotalMinutes转为总分钟数
            //TotalSeconds转为总秒数
            TimeSpan timer = GetNowRecoverTime();
            //Debug.Log($"总分钟数:{timer.TotalSeconds}");
            //Debug.Log($"总秒数:{timer.TotalMinutes}");
            //Debug.Log($"分钟数:{timer.Seconds}");
            //Debug.Log($"秒数:{timer.Minutes}");
            int minutes = (int)timer.TotalMinutes;
            int seconds = timer.Seconds;
            recoverTimeStr = $"{minutes}:{seconds:D2}";
            if (timer.TotalSeconds <= 0)
                //AddHp();
                CountDownAddHp();
        }
    }

    /// <summary>
    /// 倒计时结束恢复一点体力
    /// </summary>
    private void CountDownAddHp()
    {
        nowHp = nowHp + 1 > MAXHP ? MAXHP : nowHp + 1;
        SaveNowHp(nowHp);
    }
    
    #endregion

    #region SaveHp

    /// <summary>
    /// 存储当前体力
    /// </summary>
    /// <param name="value"></param>
    private void SaveNowHp(int value)
    {
        PlayerPrefs.SetInt("_NowHp", value);
        //发送事件通知体力变更等
        this.SendEvent<VitalityChangeEvent>(new VitalityChangeEvent());
        
    }
    
    /// <summary>
    /// 购买体力/离线体力恢复满/满体力等情况调用
    /// </summary>
    private void SaveNowHpToMax()
    {
        nowHp = MAXHP;
        recoverEndTime = DateTime.MinValue;
        recoverTimeStr = "00:00";
        SaveNowHp(MAXHP);
        SaveRecoverEndTime(string.Empty);
    }

    /// <summary>
    /// 获取当前体力
    /// </summary>
    /// <returns></returns>
    private int GetNowHp()
    {
        return PlayerPrefs.GetInt("_NowHp", MAXHP);
    }

    /// <summary>
    /// 获取体力完全恢复的时间点
    /// </summary>
    /// <returns></returns>
    private string GetRecoverEndTime()
    {
       return PlayerPrefs.GetString("_RecoverEndTime", string.Empty);
    }

    /// <summary>
    /// 保存体力完全恢复的时间点
    /// </summary>
    /// <param name="value"></param>
    private void SaveRecoverEndTime(string value)
    {
        PlayerPrefs.SetString("_RecoverEndTime", value);
    }

    #endregion

    /// <summary>
    /// 恢复一点体力
    /// </summary>
    public void AddHp()
    {
        nowHp = nowHp + 1 > MAXHP ? MAXHP : nowHp + 1;
        if (nowHp == MAXHP)
        {
            SaveNowHpToMax();
            return;
        }

        SaveNowHp(nowHp);
        //重新设置体力恢复满的时间点(用已使用体力设置)
        recoverEndTime = DateTime.Now.AddSeconds(UsedHp * RECOVERTIME);
        SaveRecoverEndTime(recoverEndTime.ToString());
    }

    /// <summary>
    /// 消耗体力的方法(消耗一点体力)
    /// </summary>
    public void UseHp()
    {
        if (nowHp > 0)
        {
            nowHp--;
            SaveNowHp(nowHp);
            if (nowHp == MAXHP - 1)
            {
                recoverEndTime = DateTime.Now.AddSeconds(RECOVERTIME);
                SaveRecoverEndTime(recoverEndTime.ToString());
            }
            else if (nowHp >= 0)
            {
                //获取体力恢复完全的时间
                string time = GetRecoverEndTime();
                DateTime lastTime = DateTime.Parse(time);
                recoverEndTime = lastTime.AddSeconds(RECOVERTIME);
                SaveRecoverEndTime(recoverEndTime.ToString());
            }
        }
        else
        {
            nowHp = 0;
            SaveNowHp(nowHp);
        }
    }

    /// <summary>
    /// 恢复满体力
    /// </summary>
    public void SetNowHpToMax()
    {
        SaveNowHpToMax();
    }
}
