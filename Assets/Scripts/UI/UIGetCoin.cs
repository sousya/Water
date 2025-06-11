using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using System.Collections;

namespace QFramework.Example
{
	public class UIGetCoinData : UIPanelData
	{
	}
	public partial class UIGetCoin : UIPanel, ICanSendEvent, ICanGetUtility, ICanGetModel
    {
        [SerializeField] private GiftPackSO[] rewardPackSO;
        //按道具顺序排序
        [SerializeField] private Sprite[] rewardSprites;    
        private StageModel stageModel;
        private SaveDataUtility saveDataUtility;
        private int curLevel;
        private int getReward;

        private const int STAR_LEVEL = 6;
        private const int END_LEVEL = 97;
        public const int REWARD_INTERVAL = 7;
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIGetCoinData ?? new UIGetCoinData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
            stageModel = this.GetModel<StageModel>();
            saveDataUtility = this.GetUtility<SaveDataUtility>();
        }

        protected override void OnShow()
        {
            getReward = -1;
            //过关后会记录当前关卡为下一关(减一表示通过的关卡)
            curLevel = saveDataUtility.GetLevelClear() - 1;
            //6-97关显示(通过97关之后不显示)
            if (curLevel >= STAR_LEVEL && curLevel < END_LEVEL)
            {
                ImgProcessNode.Show();
                int _progress = (curLevel - STAR_LEVEL + 1) % REWARD_INTERVAL;
                if (_progress == 0)
                {
                    getReward = ((curLevel - STAR_LEVEL + 1) / REWARD_INTERVAL) - 1;//减一计算索引
                    if (getReward >= 0 && getReward < rewardPackSO.Length)
                    {
                        var _packSO = rewardPackSO[getReward];
                        StartCoroutine(PlayAnimaton(_packSO));
                    }
                }

                int _displayedProgress = _progress == 0 ? REWARD_INTERVAL : _progress;
                TxtProcess.text = $"{_displayedProgress} / {REWARD_INTERVAL}";
                ImgProcess.fillAmount = (float)_displayedProgress / REWARD_INTERVAL;
            }
            BindClick();
            TxtCoin.text = ((int)(GameDefine.GameConst.WIN_COINS * stageModel.GoldCoinsMultiple)).ToString();
            TxtLevel.text = "Level " + curLevel.ToString();
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            stageModel = null;
            saveDataUtility = null;
            BtnClose.onClick.RemoveAllListeners();
            BtnContinue.onClick.RemoveAllListeners();
        }

		void BindClick()
		{
            BtnClose.onClick.AddListener(() =>
            {
                BackUIBegin();
            });

            BtnContinue.onClick.AddListener(() =>
            {
                BackUIBegin();
            });
        }

        void BackUIBegin()
        {
            this.SendEvent<LevelClearEvent>(new LevelClearEvent());
            CloseSelf();
        }

        private IEnumerator PlayAnimaton(GiftPackSO _packSO)
        {
            RewardItemManager.Instance.PrepareSlotLayout(_packSO.ItemReward.Count);
            var _actionList = new List<System.Action>();
            foreach (var item in _packSO.ItemReward)
            {
                stageModel.AddItem(item.ItemIndex, item.Quantity);
                //注意 rewardSprites 是否越界
                _actionList.Add(RewardItemManager.Instance.PlayRewardInit(rewardSprites[item.ItemIndex - 1], item.ItemIndex, item.Quantity));
            }
            //else
            //    Debug.Log($"数组越界 getReward:{ getReward }");

            foreach (var item in _actionList)
            {
                item?.Invoke();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
