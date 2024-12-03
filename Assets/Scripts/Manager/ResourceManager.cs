using GameDefine;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;

[MonoSingletonPath("[Resource]/ResourceManager")]
public class ResourceManager : MonoSingleton<ResourceManager>, ICanGetUtility, ICanSendEvent
{
    //AB包缓存---解决AB包无法重复加载的问题 也有利于提高效率。
    private Dictionary<string, AssetBundle> abCache;

    private AssetBundle mainAB = null; //主包

    private AssetBundleManifest mainManifest = null; //主包中配置文件---用以获取依赖包


    //各个平台下的基础路径 --- 利用宏判断当前平台下的streamingAssets路径
    private string basePath
    {
        get
        {
            //使用StreamingAssets路径注意AB包打包时 勾选copy to streamingAssets
#if UNITY_EDITOR || UNITY_STANDALONE
            return Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
                return Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
                return Application.dataPath + "/assets/";
#endif
        }
    }
    //各个平台下的主包名称 --- 用以加载主包获取依赖信息
    private string mainABName
    {
        get
        {
            return "AssetBundles/Android";
#if UNITY_EDITOR || UNITY_STANDALONE
            return "StandaloneWindows";
#elif UNITY_IPHONE
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#endif
        }
    }

    public void Init()
    {
        //初始化字典
        Debug.Log("初始化");
        abCache = new Dictionary<string, AssetBundle>();
  
    }

    public void LoadFont()
    {
        AssetBundle ab = LoadABPackage("font");
        Material[] lists = ab.LoadAllAssets<Material>();
        foreach (var m in lists)
        {
            m.shader = Shader.Find(m.shader.name);
        }
    }

    //加载AB包
    public AssetBundle LoadABPackage(string abName)
    {
        AssetBundle ab;
        //加载ab包，需一并加载其依赖包。
        if (mainAB == null)
        {
            //根据各个平台下的基础路径和主包名加载主包
            mainAB = AssetBundle.LoadFromFile(basePath + mainABName);
            //获取主包下的AssetBundleManifest资源文件（存有依赖信息）
            mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //根据manifest获取所有依赖包的名称 固定API
        string[] dependencies = mainManifest.GetAllDependencies(abName);
        //循环加载所有依赖包
        for (int i = 0; i < dependencies.Length; i++)
        {
            //如果不在缓存则加入
            if (!abCache.ContainsKey(dependencies[i]))
            {
                //根据依赖包名称进行加载
                ab = AssetBundle.LoadFromFile(basePath + dependencies[i]);
                //注意添加进缓存 防止重复加载AB包
                abCache.Add(dependencies[i], ab);
            }
        }
        //加载目标包 -- 同理注意缓存问题
        if (abCache.ContainsKey(abName)) return abCache[abName];
        else
        {
            ab = AssetBundle.LoadFromFile(basePath + abName);
            abCache.Add(abName, ab);
            return ab;
        }


    }

    //==================三种资源同步加载方式==================
    //提供多种调用方式 便于其它语言的调用（Lua对泛型支持不好）
    #region 同步加载的三个重载

    /// <summary>
    /// 同步加载资源---泛型加载 简单直观 无需显示转换
    /// </summary>
    /// <param name="abName">ab包的名称</param>
    /// <param name="resName">资源名称</param>
    public T Load<T>(string abName, string resName) where T : UnityEngine.Object
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset<T>(resName);
    }


    //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
    public UnityEngine.Object Load(string abName, string resName)
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset(resName);
    }


    //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
    public UnityEngine.Object Load(string abName, string resName, System.Type type)
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset(resName, type);
    }

    #endregion


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

}
