using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    private ResLoader mResLoader = ResLoader.Allocate();

    public RenderTexture renderTexture;
    protected override void Init()
    {
        //PlayerPrefs.DeleteKey("g_ClearCakeLevel");
        //PlayerPrefs.DeleteKey("g_ClearCakeChallenge");
        //PlayerPrefs.DeleteKey("g_CakeOrderBegin");
        //PlayerPrefs.DeleteKey("g_ClearCakeOrder");
        //PlayerPrefs.DeleteKey("g_CakeOrderEnd");
        //PlayerPrefs.DeleteKey("g_CakeClickChallenge");
        //PlayerPrefs.DeleteKey("g_CakeMoreStar");
        //PlayerPrefs.DeleteKey("g_CakeRankTimeEnd");
        //PlayerPrefs.DeleteKey("g_CakeStarLevel");
        //PlayerPrefs.DeleteKey("g_CakeSeven");
        //PlayerPrefs.DeleteKey("g_CakeStarLevel");
        //PlayerPrefs.SetInt("g_ClearCakeLevel", 2);
        //PlayerPrefs.SetInt("g_CakeMoreStar", PlayerPrefs.GetInt("g_CakeMoreStar") + 3);
        //PlayerPrefs.SetInt("g_ClearCakeJigsaw", 0);
        ResKit.Init();
        //ResetShader();
        RegisterModels();
        RegisterUtilitys();
        RegisterSystems();
        CreateInstance();
    }


    void RegisterModels()
    {
        RegisterModel(new StageModel());
    }

    void RegisterUtilitys()
    {
        RegisterUtility(new SaveDataUtility());
        RegisterUtility(new LanguageUtility());
    }

    void RegisterSystems()
    {
    }

    void CreateInstance()
    {
        TextManager textManager = TextManager.Instance;
        string languageStr = GetUtility<SaveDataUtility>().GetSelectLanguage();
        if (languageStr == "-1")
        {
            if(GetUtility<LanguageUtility>() == null)
            {
                Debug.Log("Ã»ÓÐUtility");
            }
            languageStr = GetUtility<LanguageUtility>().GetSystemLanguage();
            GetUtility<SaveDataUtility>().SaveSelectLanguage(languageStr);
        }
        //Debug.Log("languageStr " + languageStr);

        textManager.ReadTextCfg(languageStr);

        //LevelManager levelManager = LevelManager.Instance;
        //levelManager.ReadAllCfg();

        //ADManager aDManager = ADManager.Instance;
        //aDManager.InitializeAds();
        //ResourceManager.Instance.LoadABPackage("uieveladdheart_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelclear_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelmain_prefab");

        ShareManager shareManager = ShareManager.Instance;
        AnalyticsManager analyticsManager = AnalyticsManager.Instance;
        //LevelManager.Instance.BeginGame();
        TopOnADManager.Instance.LoadAD();
        TenjinManager.Instance.Init();
        //UIKit.OpenPanel<UIBegin>(UILevel.Common, null, "uibegin_prefab", "UIBegin");

        //ResourceManager.Instance.LoadFont();
    }


}
