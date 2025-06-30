using GameDefine;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    protected override void Init()
    {
        ResKit.Init();
        RegisterModels();
        RegisterUtilitys();
        RegisterSystems();
        CreateInstance();
        ActivityStart();
    }

    private void RegisterModels()
    {
        RegisterModel(new StageModel());
        RegisterModel(new PotionActivityModel());
        RegisterModel(new RankDataModel());
    }

    private void RegisterUtilitys()
    {
        RegisterUtility(new SaveDataUtility());
        RegisterUtility(new LanguageUtility());
    }

    private void RegisterSystems()
    {
    }

    //��������
    private void CreateInstance()
    {
        TextManager textManager = TextManager.Instance;

        //LevelManager levelManager = LevelManager.Instance;
        //levelManager.ReadAllCfg();
        //LevelManager.Instance.BeginGame();
        //TopOnADManager.Instance.LoadAD();
        //UIKit.OpenPanel<UIBegin>(UILevel.Common, null, "uibegin_prefab", "UIBegin");

        //ResourceManager.Instance.LoadABPackage("uieveladdheart_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelclear_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelmain_prefab");
        //ResourceManager.Instance.LoadFont();

        ShareManager shareManager = ShareManager.Instance;
        AnalyticsManager analyticsManager = AnalyticsManager.Instance;
        CoinManager coinManager = CoinManager.Instance;
        HealthManager healthManager = HealthManager.Instance;
        TenjinManager tenjinManager = TenjinManager.Instance;
        TopOnADManager topOnADManager = TopOnADManager.Instance;
        AvatarManager avatarManager = AvatarManager.Instance;
    }

    //�����
    private void ActivityStart()
    {
        var saveData = this.GetUtility<SaveDataUtility>();
        if (saveData.GetLevelClear() >= GameConst.WIN_STREAK_BEGIN_LEVEL)
            CountDownTimerManager.Instance.StartTimer(GameConst.RANKA_ACTIVITY_SIGN, 1440f);

    }
}
