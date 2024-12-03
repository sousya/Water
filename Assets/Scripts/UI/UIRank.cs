using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;

namespace QFramework.Example
{
	public class UIRankData : UIPanelData
	{
	}
	public partial class UIRank : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public List<RankItemCtrl> rankItems = new List<RankItemCtrl>();
		public Transform rankNode;
		public RankItemCtrl myMoveRank, myRank, first, second, third;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIRankData ?? new UIRankData();
			// please add init code here
		}

		protected override void OnOpen(IUIData uiData = null)
        {
        }
		
		protected override void OnShow()
		{
            StartCoroutine(SetRankIe());
		}

        IEnumerator SetRankIe()
        {
            rankNode.parent.parent.gameObject.SetActive(false);
            rankNode.parent.parent.gameObject.SetActive(true);
            yield return null;
            SetRank();
        }

        void Start()
        {
            SetText();
            RegisterEvent();
        }

        void RegisterEvent()
        {
            this.RegisterEvent<RefreshUITextEvent>(
                e =>
                {
                    SetText();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);

            BtnClose.onClick.AddListener(() =>
            {
                UIKit.HidePanel<UIRank>();
            });

            BtnStar.onClick.AddListener(() =>
            {
                TopOnADManager.Instance.rewardAction = AddStar;
                TopOnADManager.Instance.ShowRewardAd();


                AudioKit.PlaySound("resources://Audio/btnClick");
            });
        }
        void AddStar()
        {
            this.GetUtility<SaveDataUtility>().GetOrAddMoreStar(9);
            StartCoroutine(SetRankIe());
        }

        private void Update()
        {
            TxtTime.text = this.GetUtility<SaveDataUtility>().GetLeftTime(); 
        }

        void SetText()
		{
			TxtTitle.text = TextManager.Instance.GetConvertText("Text_RankTitle");
		}

        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public void SetRank()
        {
            rankNode.gameObject.SetActive(false);
            int star = LevelManager.Instance.GetStarNum();
			//star = 36;
			int rank = LevelManager.Instance.GetRankIdx(star);
            myMoveRank.transform.SetSiblingIndex(rank - 1);
            rankNode.gameObject.SetActive(true);

			myMoveRank.SetItem(rank, star);
			myRank.SetItem(rank, star);
        }
    }
}
