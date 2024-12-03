using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UIBeginData : UIPanelData
	{
	}
	public partial class UIBegin : UIPanel, ICanGetUtility, ICanRegisterEvent,ICanSendEvent
	{
        public float star2Num = 0.22f, star3Num = 0.4f;
		public IArchitecture GetArchitecture()
        {
			return GameMainArc.Interface;
		}

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBeginData ?? new UIBeginData();
			// please add init code here
		}

		void SetText()
        {
            //List<string> list = new List<string>();

            TxtChallenge.text = TextManager.Instance.GetConvertText("Text_ChallengeLevel");
            TxtShowCake.text = TextManager.Instance.GetConvertText("Text_ShowCake");
			TxtChallengeTitle.text = TextManager.Instance.GetConvertText("Text_ChallengeLevel");
            TxtJigsawTitle.text = TextManager.Instance.GetConvertText("Text_JigsawLevel");
            //TxtJigsawUnlock.text = TextManager.Instance.GetConvertText("Text_ChallengeUnlock", list);
            TxtContinue.text = TextManager.Instance.GetConvertText("Text_Continue");
			TxtScientist.text = TextManager.Instance.GetConvertText("Text_Scientist");
			TxtOrderTip.text = TextManager.Instance.GetConvertText("Text_UnlockOrderTip");
            TxtClear.text = TextManager.Instance.GetConvertText("Text_Clear");
            TxtGetStar.text = TextManager.Instance.GetConvertText("Text_GetStar");
            TxtGetReward.text = TextManager.Instance.GetConvertText("Text_GetReward");
            TxtLimitTime.text = TextManager.Instance.GetConvertText("Text_LimitTime");
            TxtClickContinue.text = TextManager.Instance.GetConvertText("Text_ClickContinue");
            TxtStep.text = TextManager.Instance.GetConvertText("Text_AddStep");
            SetLevel();
		}

		void CheckChange()
		{
			GoGuochang.Play("guochang");
		}

		void SetLevel()
		{
			TxtLevel.font.material.shader = Shader.Find(TxtLevel.font.material.shader.name);

            List<string> list = new List<string>();
            if (LevelManager.Instance.levelId < 9)
            {
                list.Add(9 + "");
            }
            else if(LevelManager.Instance.levelId < 19)
            {
                list.Add(19 + "");
                //TxtJigsawUnlock.text = TextManager.Instance.GetConvertText("Text_ChallengeUnlock", list);
            }
            else
            {
                list.Add(30 + "");
            }
            TxtOrderUnlock.text = TextManager.Instance.GetConvertText("Text_Challenge", list);
            if (LevelManager.Instance)
			{
				if(LevelManager.Instance.levelType == 1)
				{
                    TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_Level") + LevelManager.Instance.levelId + "";
                }
                else if(LevelManager.Instance.levelType == 2)
				{
                    TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_OrderLevel") + LevelManager.Instance.levelId + "";
                }
                else if(LevelManager.Instance.levelType == 3)
				{
                    TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_JigsawLevel") + LevelManager.Instance.levelId + "";
                }
                else if(LevelManager.Instance.levelType == 0)
                {
                    if (LevelManager.Instance.hardLevels.Contains(LevelManager.Instance.levelId))
                    {
                        TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_HardLevel") + this.GetUtility<SaveDataUtility>().GetLevelClear();
                    }
                    else if (LevelManager.Instance.spLevels.Contains(LevelManager.Instance.levelId))
                    {
                        TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_SpLevel") + this.GetUtility<SaveDataUtility>().GetLevelClear();
                    }
                    else
                    {
                        TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_Level") + this.GetUtility<SaveDataUtility>().GetLevelClear();
                    }
                }
                TxtLevel.text = TextManager.Instance.GetConvertText("Text_Level") + this.GetUtility<SaveDataUtility>().GetLevelClear();

                if (LevelManager.Instance.levelId == 2)
				{
                    TxtTeach.gameObject.SetActive(true);
					TxtTeach.text = TextManager.Instance.GetConvertText("Text_Teach");
                }
            }
			else
			{
				TxtLevel.text = TextManager.Instance.GetConvertText("Text_Level") + this.GetUtility<SaveDataUtility>().GetLevelClear();
				TxtNowLevel.text = TextManager.Instance.GetConvertText("Text_Level") + this.GetUtility<SaveDataUtility>().GetLevelClear();
			}

		}

		void RegisterEvent()
		{
			this.RegisterEvent<RefreshUITextEvent>(
				e =>
				{
					SetText();

				}).UnRegisterWhenGameObjectDestroyed(gameObject);
			this.RegisterEvent<LevelStartEvent>(
				e =>
                {
                    ImgTeach.gameObject.SetActive(false);
					if(LevelManager.Instance.levelId == 2 && LevelManager.Instance.levelType == 0)
					{
                        TeachNode.gameObject.SetActive(true);
                    }
                    else
                    {
						if(LevelManager.Instance.levelId == 3 && LevelManager.Instance.levelType == 0)
						{
							StartCoroutine(ShowOrderTip());
						}
						else
						{
                            ImgOrderTip.gameObject.SetActive(false);
                        }
                        TxtTeach.gameObject.SetActive(false);
                        TeachNode.gameObject.SetActive(false);
                    }
                    if(LevelManager.Instance.levelId == 7 && LevelManager.Instance.levelType == 0)
                    {
                        SevenNode.gameObject.SetActive(!this.GetUtility<SaveDataUtility>().GetSeven());
                        this.GetUtility<SaveDataUtility>().SaveSeven(true);
                    }
                    TxtRankNum.text = LevelManager.Instance.GetRankIdx(LevelManager.Instance.GetStarNum()) + "";
                    SetLevel();
                    CheckChange();
                    SetStar();
                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UpdateReturnEvent>(
				e =>
				{
					RefreshReturn();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<TipEvent>(
				e =>
				{
                    ShowTip(e.tipStr);

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<TeachEvent>(
				e =>
				{
					if(LevelManager.Instance.levelType == 0)
					{
                        if (e.step == 1)
                        {
                            TeachNode.gameObject.SetActive(true);
                            ImgTeach.gameObject.SetActive(true);
                            ImgTeach.transform.position = Step1Node.position;
                        }
                        else if (e.step == 2)
                        {
                            TeachNode.gameObject.SetActive(true);
                            ImgTeach.gameObject.SetActive(true);
                            ImgTeach.transform.position = Step2Node.position;
                        }
                    }
                  

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<BeginLevelEvent>(
				e =>
				{
					BeginLevel();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ShowChallengeEvent>(
				e =>
                {
                    BeginNode.gameObject.SetActive(false);
                    LevelNode.gameObject.SetActive(false);
                    ChallengeNode.gameObject.SetActive(true);
                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ShowJigsawEvent>(
				e =>
                {
					if(this.GetUtility<SaveDataUtility>().GetLevelClear() >= 19)
                    {
                        BeginNode.gameObject.SetActive(false);
                        LevelNode.gameObject.SetActive(false);
                        //JigsawChangeNode.gameObject.SetActive(false);
                        OrderNode.gameObject.SetActive(false);
                        JigsawNode.gameObject.SetActive(true);
                    }
                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<MoveCakeEvent>(
                e =>
                {
                    SetStar();
                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<LevelStartEvent>(
				e =>
                {
					int orderId = this.GetUtility<SaveDataUtility>().GetOrder();
                    var clear4 = this.GetUtility<SaveDataUtility>().GetJigsaw(4);
                    bool showRank = (LevelManager.Instance.levelId >= 7 && LevelManager.Instance.levelId <= 30) && LevelManager.Instance.levelType == 0;
                    BtnRank.gameObject.SetActive(showRank);
                    ImgStarBubbleMove.gameObject.SetActive(showRank);

                    if (((LevelManager.Instance.levelId > 2 && LevelManager.Instance.levelType == 0) || LevelManager.Instance.levelType == 2) && orderId <= 4)
                    {
                        OrderNode.gameObject.SetActive(true);
                        ImgOrderUnlock.gameObject.SetActive(LevelManager.Instance.levelId < 9 && LevelManager.Instance.levelType != 2);

                        if (LevelManager.Instance.levelType == 2)
                        {
                            ImgOrderIcon.gameObject.SetActive(false);
                            ImgHardIcon.gameObject.SetActive(false);
                            ImgNormalcon.gameObject.SetActive(true);
                            ImgJigsawIcon.gameObject.SetActive(false);
                        }
                        else
                        {

                            ImgOrderIcon.gameObject.SetActive(true);
                            ImgHardIcon.gameObject.SetActive(false);
                            ImgNormalcon.gameObject.SetActive(false);
                            ImgJigsawIcon.gameObject.SetActive(false);
                        }
                    }
                    else if ((LevelManager.Instance.levelId >= 10 && LevelManager.Instance.levelType == 0) && !clear4)
                    {
                        OrderNode.gameObject.SetActive(true);
                        ImgOrderUnlock.gameObject.SetActive(LevelManager.Instance.levelId < 19);
                        if (LevelManager.Instance.levelType == 3)
                        {
                            ImgOrderIcon.gameObject.SetActive(false);
                            ImgHardIcon.gameObject.SetActive(false);
                            ImgNormalcon.gameObject.SetActive(false);
                            ImgJigsawIcon.gameObject.SetActive(false);
                        }
                        else
                        {

                            ImgOrderIcon.gameObject.SetActive(false);
                            ImgHardIcon.gameObject.SetActive(false);
                            ImgNormalcon.gameObject.SetActive(false);
                            ImgJigsawIcon.gameObject.SetActive(true);
                        }
                    }
                    else if (LevelManager.Instance.levelId >= 20 && !this.GetUtility<SaveDataUtility>().GetClickChallenge())
                    {
                        OrderNode.gameObject.SetActive(true);
                        ImgOrderUnlock.gameObject.SetActive(LevelManager.Instance.levelId < 30);
                        if (LevelManager.Instance.levelType == 1)
                        {
                            ImgOrderIcon.gameObject.SetActive(false);
                            ImgHardIcon.gameObject.SetActive(false);
                            ImgNormalcon.gameObject.SetActive(true);
                            ImgJigsawIcon.gameObject.SetActive(false);
                        }
                        else
                        {

                            ImgOrderIcon.gameObject.SetActive(false);
                            ImgHardIcon.gameObject.SetActive(true);
                            ImgNormalcon.gameObject.SetActive(false);
                            ImgJigsawIcon.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        OrderNode.gameObject.SetActive(false);
                    }

     //               if ((LevelManager.Instance.levelId >= 10 && LevelManager.Instance.levelType == 0))
					//{
     //                   var clear4 = this.GetUtility<SaveDataUtility>().GetJigsaw(4);

					//	if(clear4)
					//	{
     //                       JigsawChangeNode.gameObject.SetActive(false);
     //                   }
     //                   else
					//	{
     //                       JigsawChangeNode.gameObject.SetActive(true);
     //                   }

     //                   if (LevelManager.Instance.levelId < 19)
					//	{
					//		ImgJigsawUnlock.gameObject.SetActive(true);
     //                   }
					//	else
					//	{
					//		ImgJigsawUnlock.gameObject.SetActive(false);
     //                   }
					//	//if(LevelManager.Instance.levelType == 3)
					//	//{
     // //                      OrderNode.gameObject.SetActive(false);
     // //                  }
     //               }
					//else
					//{
     //                   JigsawChangeNode.gameObject.SetActive(false);
     //               }

                    if (LevelManager.Instance.levelType == 2)
                    {
                        ImgOrderScroll.fillAmount = (orderId - 1) * 0.34f;

                    }

                    if (LevelManager.Instance.levelType == 3)
					{
                        JigsawNode.gameObject.SetActive(false);
                        BeginNode.gameObject.SetActive(false);
                        OrderNode.gameObject.SetActive(false);
                        LevelNode.gameObject.SetActive(true);
                    }
                }).UnRegisterWhenGameObjectDestroyed(gameObject);

            BtnLevel.onClick.AddListener(BeginLevel);
			BtnMain.onClick.AddListener(ReturnMain);
			BtnChallengeMain.onClick.AddListener(ReturnMain);
			BtnJigsawMain.onClick.AddListener(BeginLevel);
			BtnTianjia.onClick.AddListener(() =>
			{
				if (LevelManager.Instance.NoAD)
				{
					LevelManager.Instance.UnlockMoreCake();
				}
				else
				{
					TopOnADManager.Instance.rewardAction = LevelManager.Instance.UnlockMoreCake;
					TopOnADManager.Instance.ShowRewardAd();
				}
				AudioKit.PlaySound("resources://Audio/btnClick");

			});
			BtnStep.onClick.AddListener(() =>
			{
					TopOnADManager.Instance.rewardAction = LevelManager.Instance.ReduceMoveNum;
					TopOnADManager.Instance.ShowRewardAd();
				AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnChehui.onClick.AddListener(() =>
			{
				//if (LevelManager.Instance.returnNum > 0 || LevelManager.Instance.NoAD)
				if (LevelManager.Instance.returnNum > 0)
				{
					LevelManager.Instance.ReturnMove();
				}
				else
				{

					TopOnADManager.Instance.rewardAction = ReturnMove;
					TopOnADManager.Instance.ShowRewardAd();
				}
				AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnSet.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<UISet>(UILevel.Common, null, "uiset_prefab", "UISet");
				AudioKit.PlaySound("resources://Audio/btnClick");
			});

            BtnAD.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBuy>(UILevel.Common, null, "uibuy_prefab", "UIBuy");
                AudioKit.PlaySound("resources://Audio/btnClick");
            });

            BtnRefresh.onClick.AddListener(Restart);
			BtnSkip.onClick.AddListener(() =>
			{
				LevelManager.Instance.StartGame(int.Parse(InputLevel.text));
				AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnChallenge.onClick.AddListener(() =>
			{
                BeginNode.gameObject.SetActive(false);
                LevelNode.gameObject.SetActive(false);
                ChallengeNode.gameObject.SetActive(true);
				this.SendEvent<RefreshUnlockLevel>();
                AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnJigsaw.onClick.AddListener(() =>
			{
                if (this.GetUtility<SaveDataUtility>().GetLevelClear() >= 19)
                {
                    BeginNode.gameObject.SetActive(false);
                    LevelNode.gameObject.SetActive(false);
                    JigsawChangeNode.gameObject.SetActive(false);
                    JigsawNode.gameObject.SetActive(true);
                }
                this.SendEvent<RefreshUnlockLevel>();
                AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnShowCake.onClick.AddListener(() =>
			{
                UIKit.OpenPanel<UICakes>(UILevel.Common, null, "uicakes_prefab", "UICakes");
                AudioKit.PlaySound("resources://Audio/btnClick");
			});
			BtnOrder.onClick.AddListener(() =>
			{
                if (LevelManager.Instance.levelType == 2)
                {
					BeginLevel();
                }
                else
                {
					int orderId = this.GetUtility<SaveDataUtility>().GetOrder();
                    var clear4 = this.GetUtility<SaveDataUtility>().GetJigsaw(4);
                    if (orderId <= 4)
                    {
                        if (this.GetUtility<SaveDataUtility>().GetLevelClear() < 9)
                        {
                            UIKit.OpenPanel<UITips>(UILevel.Common, null, "uitips_prefab", "UITips");

                            return;
                        }
                        BeginOrderLevel();
                    }
                    else if (this.GetUtility<SaveDataUtility>().GetLevelClear() >= 19 && !clear4)
                    {
                        BeginNode.gameObject.SetActive(false);
                        LevelNode.gameObject.SetActive(false);
                        JigsawChangeNode.gameObject.SetActive(false);
                        JigsawNode.gameObject.SetActive(true);
                        this.SendEvent<RefreshUnlockLevel>();
                    }
                    else if (this.GetUtility<SaveDataUtility>().GetLevelClear() >= 30)
                    {
                        BeginNode.gameObject.SetActive(false);
                        LevelNode.gameObject.SetActive(false);
                        ChallengeNode.gameObject.SetActive(true);
                        this.GetUtility<SaveDataUtility>().SaveClickChallenge(true);
                    }
                }
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
            BtnOrderContinue.onClick.AddListener(() =>
            {
                OrderBegin.gameObject.SetActive(false);
                OrderEnd.gameObject.SetActive(false);
                BtnOrderContinue.gameObject.SetActive(false);

                if (this.GetUtility<SaveDataUtility>().GetOrder() > 4)
                {
                    BeginLevel();
                }
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
            BtnClickSeven.onClick.AddListener(() =>
            {
                SevenNode.gameObject.SetActive(false);
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
            BtnRank.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIRank>();
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
        }

        public void SetStar()
        {
            int levelId = LevelManager.Instance.levelId;
            BtnStep.gameObject.SetActive(false);
            StarNode.gameObject.SetActive(false);
            if((LevelManager.Instance.moveNum == LevelManager.Instance.nowLevel.Star3 + LevelManager.Instance.moreMoveNum - 1 ) ||(LevelManager.Instance.moveNum == LevelManager.Instance.nowLevel.Star2 + LevelManager.Instance.moreMoveNum - 1))
            {
                GoStep.Play("BtnStep");
                ImgStarBubbleMove.color = Color.red;
            }
            else
            {
                ImgStarBubbleMove.color = Color.white;
            }
            //ImgStarBubble2.gameObject.SetActive(LevelManager.Instance.moveNum == LevelManager.Instance.nowLevel.Star2 - 1);
            //ImgStarBubble3.gameObject.SetActive(LevelManager.Instance.moveNum == LevelManager.Instance.nowLevel.Star3 - 1);
            ImgStarBubble2.gameObject.SetActive(false);
            ImgStarBubble3.gameObject.SetActive(false);
            TxtStarBubble2.text = LevelManager.Instance.nowLevel.Star2 + "";
            TxtStarBubble3.text = LevelManager.Instance.nowLevel.Star3 + "";

            if ((levelId >= 7 && levelId <= 30) && !(levelId == 19 || levelId == 28 || levelId == 14))
            {
                StarNode.gameObject.SetActive(true);
                float setNum = 0;
                //BtnStep.gameObject.SetActive(true);
                int lastStar = 0;
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
                if(lastStar == 3)
                {
                    ImgScroll.fillAmount = star3Num + (1 - star3Num) * ((LevelManager.Instance.nowLevel.Star3 - LevelManager.Instance.moveNum + LevelManager.Instance.moreMoveNum) * 1f / (LevelManager.Instance.nowLevel.Star3 + LevelManager.Instance.moreMoveNum));
                }
                else if (lastStar == 2)
                {
                    float offset1 = LevelManager.Instance.nowLevel.Star2 - LevelManager.Instance.nowLevel.Star3;
                    float offset2 = -LevelManager.Instance.nowLevel.Star3 + LevelManager.Instance.moveNum - LevelManager.Instance.moreMoveNum;
                    ImgScroll.fillAmount = star2Num + (star3Num - star2Num) * ((offset1 - offset2) / offset1);
                }
                else
                {
                    float offset1 = LevelManager.Instance.nowLevel.Total - LevelManager.Instance.nowLevel.Star2;
                    float offset2 = -LevelManager.Instance.nowLevel.Star2 + LevelManager.Instance.moveNum - LevelManager.Instance.moreMoveNum;
                    ImgScroll.fillAmount = (star2Num) * ((offset1 - offset2) / offset1);
                }
                if(float.IsNaN(ImgScroll.fillAmount))
                {
                    ImgScroll.fillAmount = 1;
                }
                Vector3 setPos = new Vector3(ScrollNodeBegin.transform.localPosition.x + (ScrollNodeEnd.transform.localPosition.x - ScrollNodeBegin.transform.localPosition.x) * ImgScroll.fillAmount
                    , ImgStarBubbleMove.transform.localPosition.y, ImgStarBubbleMove.transform.localPosition.z);
                ImgStarBubbleMove.transform.localPosition = setPos;

                int moveNum = LevelManager.Instance.nowLevel.Total - LevelManager.Instance.moveNum + LevelManager.Instance.moreMoveNum;
                if(moveNum < 0)
                {
                    moveNum = 0;
                }
                TxtStarBubbleMove.text = moveNum +  "";
            }
        }

        IEnumerator ShowOrderTip()
		{
			ImgOrderTip.gameObject.SetActive(true);

			yield return new WaitForSeconds(4f);

			ImgOrderTip.gameObject.SetActive(false);
		}

		public void ShowOrderEnd()
		{
			BtnOrderContinue.gameObject.SetActive(false);
			OrderEnd.gameObject.SetActive(true);
            TxtOrderEnd.DOText(TextManager.Instance.GetConvertText("Text_OrderEnd"), 3)
				.SetEase(Ease.Linear);
            this.GetUtility<SaveDataUtility>().SaveShowOrderEnd();

            StartCoroutine(WaitCloseOrder());
        }

		void ShowOrderBegin()
		{
			BtnOrderContinue.gameObject.SetActive(false);
            OrderBegin.gameObject.SetActive(true);
            TxtOrderBegin.DOText(TextManager.Instance.GetConvertText("Text_OrderBegin"), 3)
                .SetEase(Ease.Linear);
            this.GetUtility<SaveDataUtility>().SaveShowOrderBegin();
            StartCoroutine(WaitCloseOrder());
        }

		IEnumerator WaitCloseOrder()
		{
			yield return new WaitForSeconds(4);

			BtnOrderContinue.gameObject.SetActive(true);
        }

        void ShowTip(string message)
		{
			TxtTip.text = TextManager.Instance.GetConvertText(message);

            TipNode.Play("texttip");
		}

		void RefreshReturn()
		{
			if (LevelManager.Instance.returnNum > 0)
			{
                TxtReturnNum.text = LevelManager.Instance.returnNum + "";
            }
			else
			{
                TxtReturnNum.text = "AD+3";
            }
        }

		void Restart()
		{
            AudioKit.PlaySound("resources://Audio/btnClick");
			LevelManager.Instance.ReStart();
        }

        void ReturnMove()
		{
			LevelManager.Instance.returnNum += 3;
			RefreshReturn();
        }

        void ReturnMain()
        {
            LevelManager.Instance.CancelAll();
            BeginNode.gameObject.SetActive(true);
            LevelNode.gameObject.SetActive(false);
            ChallengeNode.gameObject.SetActive(false);
            JigsawNode.gameObject.SetActive(false);
            TopOnADManager.Instance.RemoveBannerAd();
            AudioKit.PlaySound("resources://Audio/btnClick");
        }

        void BeginLevel()
		{
			LevelManager.Instance.StartGame(this.GetUtility<SaveDataUtility>().GetLevelClear());
			//LevelManager.Instance.StartGame(LevelManager.Instance.levelId);

			BeginNode.gameObject.SetActive(false);
			LevelNode.gameObject.SetActive(true);
            ChallengeNode.gameObject.SetActive(false);
			OrderLevelNode.gameObject.SetActive(false);
            JigsawNode.gameObject.SetActive(false);
            AudioKit.PlaySound("resources://Audio/btnClick");
        }

        void BeginOrderLevel()
		{
			
            if (!this.GetUtility<SaveDataUtility>().GetShowOrderBegin())
            {
                ShowOrderBegin();
            }
            LevelManager.Instance.StartOrderGame();
			//LevelManager.Instance.StartGame(LevelManager.Instance.levelId);

			BeginNode.gameObject.SetActive(false);
			LevelNode.gameObject.SetActive(true);
            ChallengeNode.gameObject.SetActive(false);
			OrderLevelNode.gameObject.SetActive(true);
            JigsawNode.gameObject.SetActive(false);
            AudioKit.PlaySound("resources://Audio/btnClick");
        }

        void Start()
		{
            RegisterEvent();
            SetText();
			RefreshReturn();
            //AudioKit.PlayMusic("resources://Audio/bgm");
        }

        void Update()
		{
			if(LevelManager.Instance != null)
			{
				
				BtnTianjia.interactable = LevelManager.Instance.moreCakeNum < 2;
				//BtnChehui.interactable = LevelManager.Instance.moveHistory.Count > 0;
				//BtnChehui.interactable = LevelManager.Instance.returnNum > 0;

            }
		}

		protected override void OnOpen(IUIData uiData = null)
        {
        }
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
