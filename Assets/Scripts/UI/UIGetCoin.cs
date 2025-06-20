using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using GameDefine;

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

            //ͨ�����߹ؿ�����ʤ�
            if (this.GetUtility<SaveDataUtility>().GetLevelClear() == 8)
                StringEventSystem.Global.Send("StartPotionActivity");

            UpdateBoxProcessNode();
            UpdateUnlockProcessNode();
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
            UIKit.ClosePanel<UIGameNode>();
            this.SendEvent<LevelClearEvent>(new LevelClearEvent());
            CloseSelf();
        }

        private void UpdateBoxProcessNode()
        {
            //���غ���¼��ǰ�ؿ�Ϊ��һ��(��һ��ʾͨ���Ĺؿ�)
            int curLevel = saveDataUtility.GetLevelClear() - 1;
            //6-97����ʾ(ͨ��97��֮����ʾ)
            if (curLevel >= STAR_LEVEL && curLevel < END_LEVEL)
            {
                ImgBoxProcessNode.Show();
                int _progress = (curLevel - STAR_LEVEL + 1) % REWARD_INTERVAL;
                int _displayedProgress = _progress == 0 ? REWARD_INTERVAL : _progress;
                TxtProcess.text = $"{_displayedProgress} / {REWARD_INTERVAL}";
                // ��ʼ��������
                int _startValue = _displayedProgress - 1;
                ImgProcess.fillAmount = (float)_startValue / REWARD_INTERVAL;

                ActionKit.Delay(0.1f, () =>
                {
                    float targetValue = (float)_displayedProgress / REWARD_INTERVAL;
                    ImgProcess.DOFillAmount(targetValue, 0.5f).SetEase(Ease.OutQuad);
                }).Start(this);

                if (_progress == 0)
                {
                    getReward = ((curLevel - STAR_LEVEL + 1) / REWARD_INTERVAL) - 1;//��һ��������
                    if (getReward >= 0 && getReward < rewardPackSO.Length)
                    {
                        var _packSO = rewardPackSO[getReward];
                        StartCoroutine(RewardItemManager.Instance.PlayRewardAnim(_packSO));
                    }
                }
            }
            TxtCoin.text = ((int)(GameDefine.GameConst.WIN_COINS * stageModel.GoldCoinsMultiple)).ToString();
            TxtLevel.text = "Level " + curLevel.ToString();

            //��ʤ�����״̬
            if (!CountDownTimerManager.Instance.IsTimerFinished(GameConst.POTION_ACTIVITY_SIGN))
            {
                var potionActivityModel = this.GetModel<PotionActivityModel>();
                potionActivityModel.AddPotionActivityGoal();
            }
        }

        private void UpdateUnlockProcessNode()
        {
            int curLevel = saveDataUtility.GetLevelClear();

            // �ҵ���һ������Ŀ��
            for (int i = 0; i < UNLOCKLEVEL.Length; i++)
            {
                if (curLevel <= UNLOCKLEVEL[i])
                {
                    ImgUnlockProcessNode.Show();
                    ImgUnlock.sprite = unlockSprites[i];

                    int prevUnlock = (i == 0) ? 0 : UNLOCKLEVEL[i - 1]; // ��һ��������
                    int totalNeeded = UNLOCKLEVEL[i] - prevUnlock;      // ��Ҫ��ɵĹؿ���
                    int currentProgress = curLevel - prevUnlock;        // ��ǰ����

                    if (currentProgress > totalNeeded)
                        currentProgress = totalNeeded;

                    TxtUnlockProcess.text = $"{currentProgress} / {totalNeeded}";

                    int startValue = currentProgress - 1;
                    ImgUnlockProcess.fillAmount = (float)startValue / totalNeeded; ;

                    ActionKit.Delay(0.1f, () =>
                    {
                        float targetValue = (float)currentProgress / totalNeeded;
                        ImgUnlockProcess.DOFillAmount(targetValue, 0.5f).SetEase(Ease.OutQuad);
                        //ImgUnlockProcess.fillAmount = (float)currentProgress / totalNeeded;
                    }).Start(this);
                   

                    return;
                }
            }

            // ���л����ѽ��������ؽ���UI
            ImgUnlockProcessNode.Hide();
        }
    }
}
