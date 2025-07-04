using GameDefine;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCtrl : MonoBehaviour, ICanSendEvent
{
    public static GameCtrl Instance;
    public BottleCtrl FirstBottle, SecondBottle;

    public bool control = false;

    //ѡ�е��߱�־�����߷���
    [SerializeField] private bool isSelectedItem = false;    
    private Action<BottleCtrl> RandomItemAction;

    //��ˮ����������0��ʾ��ǰ�����ڵ�ˮ����
    private int pouringCount = 0;
    public bool IsPouring => pouringCount == 0;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Application.targetFrameRate = 5;
    }

    /// <summary>
    /// ѡ��ƿ��
    /// </summary>
    /// <param name="bottle"></param>
    public void OnSelect(BottleCtrl bottle)
    {
        if (isSelectedItem)
        {
            //���Խ�����ID���룬���ݵ���ID�����ֲ�ͬ���

            //�ܱ�ѡ�С�ˮ�����1�Ҳ�ͬɫ���������ˮ��
            if (bottle.OnSelect(false)
            && bottle.waters.Count > 1 && !bottle.waters.All(x => x == bottle.waters[0])
            && bottle.waterItems.All(x => x != WaterItem.Ice))
            {
                //Debug.Log("��ѡ��");
                RandomItemAction?.Invoke(bottle);
            }
            else
            {
                //Debug.Log("����ѡ��");
                LevelManager.Instance.HideItemSelect();
            }
            isSelectedItem = false;
            RandomItemAction = null;
        }
        else
        {
            if (!control)
            {

                if (FirstBottle == null)
                {
                    if (bottle.OnSelect(true))
                    {
                        AudioKit.PlaySound("resources://Audio/SelectBottle");
                        FirstBottle = bottle;
                    }

                }
                else if (SecondBottle == null)
                {

                    if (bottle != FirstBottle)// && bottle.OnSelect(false)
                    {
                        SecondBottle = bottle;
                    }
                    else
                    {
                        FirstBottle.OnCancelSelect();
                        FirstBottle = null;
                    }
                }

                if (FirstBottle != null && SecondBottle != null)
                {
                    control = true;
                    if (FirstBottle.CheckMoveOut() && SecondBottle.CheckMoveIn(FirstBottle.GetMoveOutTop())
                        && !FirstBottle.isPlayAnim && !SecondBottle.isPlayAnim)
                    {
                        // ը�����ж�������ˮƿ�����ݣ��̽������ƶ���ǰ��
                        LevelManager.Instance.AddMoveNum();
                        // ը�����²�����ʧ�ܼ��
                        LevelManager.Instance.BombUpdate(); 
                        //Debug.Log("�ƶ� " + FirstCake.gameObject.name + "->" + SecondCake.gameObject.name);
                        LevelManager.Instance.RecordLast();
                        ++pouringCount;
                        FirstBottle.MoveTo(SecondBottle);
                        FirstBottle = null;
                        SecondBottle = null;
                        AudioKit.PlaySound("resources://Audio/PourWaterSound");
                        //LevelManager.Instance.AddMoveNum();//����ͳ��.��ʱ����

                        // ����ͳ��
                       
                    }
                    else
                    {
                        control = false;
                        FirstBottle.OnCancelSelect();
                        FirstBottle = null;
                        SecondBottle = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ѡ�е���״̬
    /// </summary>
    /// <param name="action"></param>
    public void SeletedItem(Action<BottleCtrl> action)
    {
        isSelectedItem = true;
        RandomItemAction = action;  
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public void InitGameCtrl()
    {
        FirstBottle = null;
        SecondBottle = null;
        control = false;
    }

    /// <summary>
    /// ��ˮ״̬���
    /// </summary>
    public void ReducePouringCount()
    {
        --pouringCount;
        if (pouringCount < 0)
            pouringCount = 0;
    }

    /// <summary>
    /// ���õ�ˮ״̬
    /// </summary>
    public void InitPouringCount()
    {
        pouringCount = 0;
    }
}
