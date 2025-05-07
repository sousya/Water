using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class StageModel : AbstractModel
{
    /// <summary>
    /// 当前关卡
    /// </summary>
    public int level = 0;
    protected override void OnInit()
    {
    }

    public void StartStage(int stageNum)
    {
        GameObject go = Resources.Load<GameObject>("Prefab/Stages/Stage" + stageNum);
        GameObject.Instantiate(go);
    }

}
