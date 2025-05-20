using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    protected override void Init()
    {
        ResKit.Init();
        //RegisterModels();
        RegisterUtilitys();
        RegisterSystems();
        CreateInstance();
    }

    /*//暂无数据层
    void RegisterModels()
    {
        RegisterModel(new StageModel());
    }*/

    void RegisterUtilitys()
    {
        RegisterUtility(new SaveDataUtility());
        RegisterUtility(new LanguageUtility());
    }

    void RegisterSystems()
    {
    }

    //单例构建
    void CreateInstance()
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
    }
}
