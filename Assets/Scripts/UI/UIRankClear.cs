using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

namespace QFramework.Example
{
	public class UIRankClearData : UIPanelData
	{
	}
	public partial class UIRankClear : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public List<RankItemCtrl> rankItemCtrls = new List<RankItemCtrl>();
		public Transform rankNode, fillItem;
		public RankItemCtrl myItem;
		public ScrollRect scroll;
        public int nextRank;
        public List<Transform> posItem = new List<Transform>();
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIRankClearData ?? new UIRankClearData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			BtnContinue.onClick.RemoveAllListeners();
			BtnContinue.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIClear>(UILevel.Common, null, "uiclear_prefab", "UIClear");
                UIKit.ClosePanel<UIRankClear>();
            });
            this.RegisterEvent<RefreshUITextEvent>(
			 e =>
			 {
				 SetText();

			 }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        void SetText()
        {
            TxtContinue.text = TextManager.Instance.GetConvertText("Text_Continue");
            TxtExcellent.text = TextManager.Instance.GetConvertText("Text_Excellent");
        }

        protected override void OnShow()
        {
            SetText();
            StartCoroutine(BeginShow());
		}

        IEnumerator BeginShow()
		{
			yield return new WaitForEndOfFrame();


            int star = LevelManager.Instance.GetStarNum();
            //star = 36;
            int lastStar = 0;
            if (LevelManager.Instance.nowLevel.Star3 != 0)
            {
                if (LevelManager.Instance.moveNum < LevelManager.Instance.nowLevel.Star3 + LevelManager.Instance.moreMoveNum)
                {
                    lastStar = 3;
                }
                else if (LevelManager.Instance.moveNum < LevelManager.Instance.nowLevel.Star2 + LevelManager.Instance.moreMoveNum)
                {
                    lastStar = 2;
                }
                else
                {
                    lastStar = 1;
                }
            }
            //int rank = LevelManager.Instance.GetRankIdx(star);
            //nextRank = LevelManager.Instance.GetRankIdx(star + lastStar);
            int rank = LevelManager.Instance.GetRankIdx(star - lastStar);
            int lastRank = LevelManager.Instance.GetRankIdx(star - lastStar);
            nextRank = LevelManager.Instance.GetRankIdx(star);
            //int rank = 70;
            //nextRank = 1;
            if (nextRank >= 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    rankItemCtrls[7 - i].SetName(nextRank - i);
                }


                for (int i = 0; i < 32; i++)
                {
                    rankItemCtrls[8 + i].SetName(nextRank + i);
                }
            }
            else
            {
                fillItem.SetSiblingIndex(nextRank - 1);
                for (int i = 0; i < nextRank; i++)
                {
                    rankItemCtrls[nextRank - 1 - i].SetName(nextRank - i);
                }

                for (int i = 0; i < 40 - nextRank; i++)
                {
                    rankItemCtrls[nextRank + i].SetName(nextRank + 1 + i);
                }
            }

            if(lastRank > 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    rankItemCtrls[39 + i].SetName(rank - i - 1);
                }
                myItem.isSp = true;
                myItem.SetItem(nextRank, star);
                myItem.SetMy(nextRank);

                ShowAnim();
            }
            else
            {
                //for (int i = 0; i < lastRank; i++)
                //{
                //    rankItemCtrls[39 + i].SetName(rank - i - 1);
                //}
                fillItem.SetSiblingIndex(lastRank - 1);
                scroll.DOVerticalNormalizedPos(lastRank / 60, 0.1f).OnComplete(() =>
                {
                    myItem.isSp = true;
                    myItem.SetItem(nextRank, star);
                    myItem.SetMy(nextRank);
                    ShowAnim();
                });
            }
        }

		public void ShowAnim()
		{
			myItem.transform.parent = transform;
            //fillItem.transform.parent = rankNode;
            //fillItem.transform.SetSiblingIndex(8);

            if (nextRank >= 8)
            {
               myItem.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).OnComplete(
                   () =>
                   {
                       myItem.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.Linear); ;
                   }).SetEase(Ease.Linear);
            }
            else
            {
                fillItem.SetSiblingIndex(nextRank - 1);
                StartCoroutine(MoveScrool());
            }
               
            scroll.DOVerticalNormalizedPos(1, 1).OnComplete(
				() =>
				{
					fillItem.gameObject.SetActive(false);
                    fillItem.parent = transform;
                    myItem.transform.parent = rankNode;
                    if(nextRank >= 8)
                    {
                        myItem.transform.SetSiblingIndex(7);
                    }
                    else
                    {
                        myItem.transform.SetSiblingIndex(nextRank - 1);

                        foreach (var item in rankItemCtrls)
                        {
                            item.Reset();
                        }
                    }

                });
			//fillItem.transform.parent = transform;
			//myItem.transform.parent = rankNode;
   //         myItem.transform.SetSiblingIndex(8);

        }

        IEnumerator MoveScrool()
        {
            yield return new WaitForEndOfFrame();
            myItem.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.4f).OnComplete(
               () =>
               {
                   myItem.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f).SetEase(Ease.Linear); ;
               }).SetEase(Ease.Linear);
            myItem.transform.DOLocalMoveY(posItem[nextRank - 1].localPosition.y, 0.4f).SetEase(Ease.Linear);
        }
        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
