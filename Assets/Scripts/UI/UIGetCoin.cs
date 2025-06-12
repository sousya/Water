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
        [SerializeField] private Sprite[] unlockSprites;
        private StageModel stageModel;
        private SaveDataUtility saveDataUtility;
        private int getReward;

        private const int STAR_LEVEL = 6;
        private const int END_LEVEL = 97;
        private const int REWARD_INTERVAL = 7;

        private readonly int[] UNLOCKLEVEL = new int[] { 7, 11, 16, 21, 24, 31, 51, 61, 91 };

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
            BindClick();
            getReward = -1;
            

            UpdateBoxProcessNode();
            UpdateUnlockProcessNode();


            //ImgUnlockProcessNode.Show();
            //ImgUnlock.sprite = unlockSprites[0];
            //TxtUnlockProcess.text = curLevel.ToString();
            //ImgUnlockProcess.fillAmount = 0f;
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

        private void UpdateBoxProcessNode()
        {
            //过关后会记录当前关卡为下一关(减一表示通过的关卡)
            int curLevel = saveDataUtility.GetLevelClear() - 1;
            //6-97关显示(通过97关之后不显示)
            if (curLevel >= STAR_LEVEL && curLevel < END_LEVEL)
            {
                ImgBoxProcessNode.Show();
                int _progress = (curLevel - STAR_LEVEL + 1) % REWARD_INTERVAL;
                if (_progress == 0)
                {
                    getReward = ((curLevel - STAR_LEVEL + 1) / REWARD_INTERVAL) - 1;//减一计算索引
                    if (getReward >= 0 && getReward < rewardPackSO.Length)
                    {
                        var _packSO = rewardPackSO[getReward];
                        StartCoroutine(RewardItemManager.Instance.PlayRewardAnim(_packSO));
                    }
                }

                int _displayedProgress = _progress == 0 ? REWARD_INTERVAL : _progress;
                TxtProcess.text = $"{_displayedProgress} / {REWARD_INTERVAL}";
                ImgProcess.fillAmount = (float)_displayedProgress / REWARD_INTERVAL;
            }
            TxtCoin.text = ((int)(GameDefine.GameConst.WIN_COINS * stageModel.GoldCoinsMultiple)).ToString();
            TxtLevel.text = "Level " + curLevel.ToString();
        }

        private void UpdateUnlockProcessNode()
        {
            int curLevel = saveDataUtility.GetLevelClear();

            // 找到下一个解锁目标
            for (int i = 0; i < UNLOCKLEVEL.Length; i++)
            {
                if (curLevel <= UNLOCKLEVEL[i])
                {
                    ImgUnlockProcessNode.Show();

                    ImgUnlock.sprite = unlockSprites[i];

                    if (curLevel == UNLOCKLEVEL[i])
                    {
                        // 已达成解锁目标，满进度
                        TxtUnlockProcess.text = $"{UNLOCKLEVEL[i]} / {UNLOCKLEVEL[i]}";
                        ImgUnlockProcess.fillAmount = 1f;
                    }
                    else
                    {
                        // 解锁中，进度计算
                        TxtUnlockProcess.text = $"{curLevel} / {UNLOCKLEVEL[i]}";
                        ImgUnlockProcess.fillAmount = (float)curLevel / UNLOCKLEVEL[i];
                    }

                    return;
                }
            }
            // 所有机制已解锁，隐藏解锁UI
            ImgUnlockProcessNode.Hide();
        }
    }
}
