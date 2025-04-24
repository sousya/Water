using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[MonoSingletonPath("[Health]/HealthManager")]
public class HealthManager : MonoSingleton<HealthManager> ,ICanGetUtility
{
    private const int maxHp = 5;
    public int nowHp;
    public bool useHp = true;//用于后续无线体力功能

    public int NowHp
    {
        get { return nowHp; }
    }

    //体力恢复一点的所需时间（60秒*30）
    private float recoverTime = 60 * 10;
    //体力完全恢复的时间点
    public DateTime recoverEndTime;
    //当前体力恢复剩余时间
    public string recoverTimeStr;

    public override void OnSingletonInit()
    {
        Init();
    }

    private void Init()
    {
        //获取当前体力
        nowHp = PlayerPrefs.GetInt("_NowHp", maxHp);
        //获取体力恢复完全的时间
        string time = PlayerPrefs.GetString("_RecoverEndTime", string.Empty);
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
    void CheckRecoverHp()
    {
        //如果是默认值 说明没消耗体力 不继续检测
        if (recoverEndTime == DateTime.MinValue)
            return;

        //获取恢复所有体力的所需时间
        float timer = GetrecoverTime();
        //如果这个结果是负的 说明体力恢复完成
        if (timer <= 0)
        {
            nowHp = maxHp;
            PlayerPrefs.SetInt("_NowHp", maxHp);
            recoverEndTime = DateTime.MinValue;
            PlayerPrefs.SetString("_RecoverEndTime", string.Empty);
        }
        //timer大于0 ，仍需要倒计时恢复体力
        else
        {
            //计算还需要恢复多少点体力 ， 回满所需时间 / 一点体力恢复时间  =》 向上取整
            int num = (int)Math.Ceiling(timer / recoverTime);
            //当回满所需值 num大于或者等于5时(理论不会出现大于maxHp情况) ，代表体力一定是0点
            if (num >= maxHp)
                nowHp = 0;

            //否则最大值 - 回满所需值 = 当前值
            else
                nowHp = maxHp - num;
            //保存体力值
            PlayerPrefs.SetInt("_NowHp", nowHp);
        }

        //Debug.Log(recoverEndTime);
    }

    /// <summary>
    /// 获取恢复所有体力的所需时间
    /// </summary>
    /// <returns></returns>
    float GetrecoverTime()
    {
        //获取当前时间与 恢复完成时间的时间间隔
        TimeSpan recoverInterval = recoverEndTime - DateTime.Now;
        // 将时间转为秒
        float remainingTime = (float)recoverInterval.TotalSeconds;
        return remainingTime;
    }

    //获取当前这一点体力的恢复时间
    TimeSpan GetNowRecoverTime()
    {
        // 计算当前体力对应的恢复时间
        int hpDifference = maxHp - nowHp - 1;
        DateTime nowRecoverTime = recoverEndTime.AddSeconds(-recoverTime * hpDifference);
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
        if (nowHp < maxHp)
        {
            TimeSpan timer = GetNowRecoverTime();
            int minutes = (int)timer.TotalMinutes;
            int seconds = timer.Seconds;
            recoverTimeStr = $"{minutes}:{seconds:D2}";
            if (timer.TotalSeconds <= 0)
            {
                //体力加一
                AddHp();

            }
        }
    }

    void AddHp()
    {
        nowHp = nowHp + 1 > maxHp ? maxHp : nowHp + 1;
        PlayerPrefs.SetInt("_NowHp", nowHp);
    }

    //消耗体力的方法
    public void UseHp()
    {
        if (nowHp > 0)
        {
            nowHp--;
            PlayerPrefs.SetInt("_NowHp", nowHp);
            //如果是第一次消耗体力
            if (nowHp == maxHp - 1)
            {
                //体力完全恢复时间
                recoverEndTime = DateTime.Now.AddSeconds(recoverTime);
                //保存恢复完成时间
                PlayerPrefs.SetString("_RecoverEndTime", recoverEndTime.ToString());
            }
            else if (nowHp >= 0)  //不是第一次消耗体力
            {
                //获取体力恢复完全的时间
                string time = PlayerPrefs.GetString("_RecoverEndTime", string.Empty);
                DateTime lastTime = DateTime.Parse(time);
                recoverEndTime = lastTime.AddSeconds(recoverTime);
                //保存恢复完成时间
                PlayerPrefs.SetString("_RecoverEndTime", recoverEndTime.ToString());
            }
        }
        else
        {
            nowHp = 0;
            PlayerPrefs.SetInt("_NowHp", 0);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}
