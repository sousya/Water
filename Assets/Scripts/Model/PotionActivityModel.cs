using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class PotionActivityModel : AbstractModel, ICanGetModel
{
    private StageModel stageModel;
    private SaveDataUtility saveDataUtility;

    //�����
    public int PotionActivityGoal => mPotionActivityGoal.Value;
    private BindableProperty<int> mPotionActivityGoal;

    //�����(�ڼ�������)
    public int PotionActivityProgress => mPotionActivityProgress.Value;
    private BindableProperty<int> mPotionActivityProgress;

    //�嵵��ʤ���ӻ���
    private int WinStreakPoints => stageModel.CountinueWinNum switch
    {
        >= 5 => 100,
        4 => 25,
        3 => 10,
        2 => 5,
        1 => 1,
        _ => 0
    };

    //���ʤ��λӳ��(��������)
    public int WinStreakLevel => stageModel.CountinueWinNum switch
    {
        >= 5 => 4,
        4 => 3,
        3 => 2,
        2 => 1,
        1 => 0,
        _ => 0
    };

    private const string POTION_ACTIVITY_GOAL_SIGN = "PotionActivityGoal";
    private const string POTION_ACTIVITY_PROGRESS_SIGN = "PotionActivityProgress";


    protected override void OnInit()
    {
        stageModel = this.GetModel<StageModel>();
        saveDataUtility = this.GetUtility<SaveDataUtility>();
        mPotionActivityGoal = new BindableProperty<int>();
        mPotionActivityProgress = new BindableProperty<int>();

        mPotionActivityGoal.SetValueWithoutEvent(saveDataUtility.LoadIntValue(POTION_ACTIVITY_GOAL_SIGN));
        mPotionActivityGoal.Register(value =>
        {
            saveDataUtility.SaveInt(POTION_ACTIVITY_GOAL_SIGN, value);
        });

        mPotionActivityProgress.SetValueWithoutEvent(saveDataUtility.LoadIntValue(POTION_ACTIVITY_PROGRESS_SIGN));
        mPotionActivityProgress.Register(value =>
        {
            saveDataUtility.SaveInt(POTION_ACTIVITY_PROGRESS_SIGN, value);
        });
    }

    public void AddPotionActivityGoal()
    {
        mPotionActivityGoal.Value += WinStreakPoints;
        //mPotionActivityGoal.Value += 500;
    }

    public void ReducePotionActivityGoal(int goal)
    {
        mPotionActivityGoal.Value -= goal;
    }

    public void AddPotionActivityProgress()
    {
        mPotionActivityProgress.Value += 1;
    }
}
