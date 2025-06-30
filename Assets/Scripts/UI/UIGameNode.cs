using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using GameDefine;
using System.Collections.Generic;
using System.Linq;

namespace QFramework.Example
{
	public class UIGameNodeData : UIPanelData
	{
	}

	public partial class UIGameNode : UIPanel ,IController
	{
        private StageModel stageModel;

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIGameNodeData ?? new UIGameNodeData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
            //���ģʽ�£�AssetBundle ������Դ����Ҫ��������
            TxtLevel.font.material.shader = Shader.Find(TxtLevel.font.material.shader.name);
            TxtLevel.text = LevelManager.Instance.levelId.ToString();
            stageModel = this.GetModel<StageModel>();
        }
		
		protected override void OnShow()
		{
            SetTakeItem();
            SetItem();
            BindBtn();
            RegisterEvent();
        }

        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            stageModel = null;
            BtnStepBack.onClick.RemoveAllListeners();
            BtnRemoveHide.onClick.RemoveAllListeners();
            BtnAddBottle.onClick.RemoveAllListeners();
            BtnHalfBottle.onClick.RemoveAllListeners();
            BtnRemoveAll.onClick.RemoveAllListeners();
            BtnReturn.onClick.RemoveAllListeners();
            BtnItem1.onClick.RemoveAllListeners();
            BtnItem2.onClick.RemoveAllListeners();
            BtnItem3.onClick.RemoveAllListeners();
        }

        private void BindBtn()
		{
            BtnStepBack.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[1] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 1 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    if (LevelManager.Instance.ReturnLast())
                        stageModel.ReduceItem(1, 1);
                }
            });

            BtnRemoveHide.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[2] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 2 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    //�ж��Ƿ��к�ˮƿ
                    if (LevelManager.Instance.hideBottleList.Count != 0)
                    {
                        LevelManager.Instance.RemoveHide(() =>
                        {
                            stageModel.ReduceItem(2, 1);
                        });
                    }
                }
            });

            BtnAddBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[3] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 3 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(false, () =>
                    {
                        stageModel.ReduceItem(3, 1);
                    });
                }
            });

            BtnHalfBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[4] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 4 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        stageModel.ReduceItem(4, 1);
                    });
                }
            });

            BtnRemoveAll.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[5] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 5 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    if (LevelManager.Instance.CheckAllDebuff())
                    {
                        LevelManager.Instance.RemoveAll(() =>
                        {
                            //��ղ�����¼���ϰ�(������˻ָ�)
                            foreach (var bottle in LevelManager.Instance.nowBottles)
                            {
                                foreach (var record in bottle.moveRecords)
                                {
                                    record.isFreeze = false;
                                    record.isClearHide = false;
                                    record.isNearHide = false;
                                    record.limitColor = 0;

                                    for (int i = 0; i < record.hideWaters.Count; i++)
                                    {
                                        record.hideWaters[i] = false;
                                    }

                                    for (int i = 0; i < record.waterItems.Count; i++)
                                    {
                                        record.waterItems[i] = WaterItem.None;
                                    }
                                }
                            }
                        });
                        stageModel.ReduceItem(5, 1);
                    }
                }
            });

            BtnReturn.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIRetry>();
            });

            BtnItem1.onClick.AddListener(() =>
            {
                UseItem(6, BtnItem1);
            });
            BtnItem2.onClick.AddListener(() =>
            {
                UseItem(7, BtnItem2);
            });
            BtnItem3.onClick.AddListener(() =>
            {
                LevelManager.Instance.ShowItemSelect();
                GameCtrl.Instance.SeletedItem(bottele => { UseItem(8, BtnItem3, bottele); });
            });
        }

        private void RegisterEvent()
        {
            this.RegisterEvent<RefreshItemEvent>(e =>
            {
                SetItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<LevelStartEvent>(e =>
            {
                SetTakeItem();
                TxtLevel.text = LevelManager.Instance.levelId.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject); 

            StringEventSystem.Global.Register("StreakWinItem", (int count) =>
            {
                ClearBottleBlackWater(count ,useItem:false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #region �������

        /// <summary>
        /// ʹ��Я������
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemObj"></param>
        /// <param name="botter">�������ĸ�ƿ��(����ˮ����ߴ���)</param>
        void UseItem(int itemID, Button itemObj, BottleCtrl botter = null)
        {
            switch (itemID)
            {
                case 6:
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        if (CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))//!HealthManager.Instance.UnLimitHp
                            stageModel.ReduceItem(6, 1);
                        TxtItem1.text = "0";
                    });
                    break;

                case 7:
                    if (!(LevelManager.Instance.hideBottleList.Count > 0))
                        return;
                    ClearBottleBlackWater(2, true, () =>
                    {
                        if (CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))
                            stageModel.ReduceItem(7, 1);
                        TxtItem2.text = "0";
                    });
                    break;

                case 8:
                    // �����б��������ϴ��
                    List<int> _indices = Enumerable.Range(0, botter.waters.Count).ToList();
                    do
                    {
                        for (int i = 0; i < _indices.Count; i++)
                        {
                            int randIndex = UnityEngine.Random.Range(i, _indices.Count);
                            (_indices[i], _indices[randIndex]) = (_indices[randIndex], _indices[i]);
                        }
                    }
                    while (Enumerable.SequenceEqual(_indices.Select(i => botter.waters[i]), botter.waters));

                    List<int> _newWaters = new List<int>();
                    List<bool> _newHideWater = new List<bool>();
                    List<WaterItem> _newWaterItems = new List<WaterItem>();

                    foreach (int idx in _indices)
                    {
                        _newWaters.Add(botter.waters[idx]);
                        _newHideWater.Add(botter.hideWaters[idx]);
                        _newWaterItems.Add(botter.waterItems[idx]);
                    }
                    // �滻ԭ�б�
                    botter.waters = _newWaters;
                    botter.hideWaters = _newHideWater;
                    botter.waterItems = _newWaterItems;

                    //�޸�ˮ����ɫ���л�����λ��
                    for (int i = 0; i < botter.waters.Count; i++)
                    {
                        var useColor = botter.waters[i] - 1;
                        if (useColor < 1000)
                            botter.waterImg[i].SetColorState(ItemType.UseColor, LevelManager.Instance.waterColor[useColor], i == botter.topIdx);
                        else
                            botter.waterImg[i].SetColorState((ItemType)botter.waters[i], LevelManager.Instance.ItemColor, i == botter.topIdx);
                    }

                    //�޸�ˮ��λ�ã��޸�ˮ����ɫ������ˮ�涯��
                    botter.SetNowSpinePos(botter.waters.Count);
                    botter.PlaySpineWaitAnim();
                    botter.CheckWaterItem();

                    botter.SetHideShow(true);
                    LevelManager.Instance.HideItemSelect();

                    if (CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))
                        stageModel.ReduceItem(8, 1);
                    TxtItem3.text = "0";
                    //Debug.Log("����˳��ɹ�");
                    break;
            }

            //if (!CheckHaveItem(itemID))//����Ϊ��ʹ��һ��
            itemObj.interactable = false;
        }

        /// <summary>
        /// ���ƿ�����к�ˮ
        /// </summary>
        /// <param name="count">�����ƿ������</param>
        /// <param name="effctNow">�Ƿ�������Ч</param>
        /// <param name="action">�ص�(����ʹ��ʱ����)</param>
        private void ClearBottleBlackWater(int count, bool effctNow = false, Action action = null, bool useItem = true)
        {
            if (LevelManager.Instance.hideBottleList.Count > 0)
            {
                var tempList = new List<BottleCtrl>(LevelManager.Instance.hideBottleList);

                while (tempList.Count > count)
                {
                    int randIndex = UnityEngine.Random.Range(0, tempList.Count);
                    tempList.RemoveAt(randIndex);
                }
                foreach (var item in tempList)
                {
                    if (!useItem)
                        item.StarSetHideShow();
                    else
                    {
                        for (int i = 0; i < item.hideWaters.Count; i++)
                        {
                            item.hideWaters[i] = false;
                        }
                        item.SetHideShow(effctNow);
                    }
                    LevelManager.Instance.hideBottleList.Remove(item);
                }

                action?.Invoke();
            }
        }

        /// <summary>
        /// ʹ��Я�����߰�ť�¼�
        /// </summary>
        /// ������Ϸ/���ùؿ�����
        void SetTakeItem()
        {
            var takeItems = LevelManager.Instance.takeItem;

            var buttons = new[] { BtnItem1, BtnItem2, BtnItem3 };
            var texts = new[] { TxtItem1, TxtItem2, TxtItem3 };
            var itemIds = new[] { 6, 7, 8 };

            for (int i = 0; i < itemIds.Length; i++)
            {
                int itemId = itemIds[i];

                bool active = (takeItems.Contains(itemId) && CheckHaveItem(itemId)) 
                    || !CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN);
                buttons[i].interactable = active;
                texts[i].text = active ? "1" : "0";
            }
        }

        /// <summary>
        /// �·����������߸���
        /// </summary>
        private void SetItem()
        {
            BtnAddStepBack.gameObject.SetActive(stageModel.ItemDic[1] <= 0);
            TxtRefreshNum.text = stageModel.ItemDic[1].ToString();

            BtnAddRemove.gameObject.SetActive(stageModel.ItemDic[2] <= 0);
            TxtRemoveHideNum.text = stageModel.ItemDic[2].ToString();

            BtnAddAddBottle.gameObject.SetActive(stageModel.ItemDic[3] <= 0);
            TxtAddBottleNum.text = stageModel.ItemDic[3].ToString();

            BtnAddHalfBottle.gameObject.SetActive(stageModel.ItemDic[4] <= 0);
            TxtAddHalfBottleNum.text = stageModel.ItemDic[4].ToString();

            BtnAddRemoveBottle.gameObject.SetActive(stageModel.ItemDic[5] <= 0);
            TxtRemoveAllNum.text = stageModel.ItemDic[5].ToString();
        }

        /// <summary>
        /// ����Ƿ�ӵ�е���
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool CheckHaveItem(int itemID)
        {
            if (stageModel.ItemDic[itemID] > 0)
                return true;
            else return false;
        }

        #endregion
    }
}
