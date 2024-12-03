using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:b0f97780-b486-45aa-a8f8-447891266fa6
	public partial class UIBegin
	{
		public const string Name = "UIBegin";
		
		[SerializeField]
		public Transform TeachNode;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTeach;
		[SerializeField]
		public UnityEngine.UI.Image ImgTeach;
		[SerializeField]
		public Transform Step2Node;
		[SerializeField]
		public Transform Step1Node;
		[SerializeField]
		public Animator TipNode;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTip;
		[SerializeField]
		public Transform JigsawChangeNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnJigsaw;
		[SerializeField]
		public UnityEngine.UI.Image ImgJigsawUnlock;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtJigsawUnlock;
		[SerializeField]
		public Transform LevelNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnMain;
		[SerializeField]
		public UnityEngine.UI.Button BtnRefresh;
		[SerializeField]
		public UnityEngine.UI.Button BtnChehui;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtReturnNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnTianjia;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnRank;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRankNum;
		[SerializeField]
		public Animator GoStep;
		[SerializeField]
		public UnityEngine.UI.Button BtnStep;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStep;
		[SerializeField]
		public RectTransform StarNode;
		[SerializeField]
		public RectTransform ScrollNodeBegin;
		[SerializeField]
		public RectTransform ScrollNodeEnd;
		[SerializeField]
		public UnityEngine.UI.Image ImgScrollBg;
		[SerializeField]
		public UnityEngine.UI.Image ImgScroll;
		[SerializeField]
		public UnityEngine.UI.Image ImgStarBubbleMove;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStarBubbleMove;
		[SerializeField]
		public UnityEngine.UI.Image ImgStarBubble2;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStarBubble2;
		[SerializeField]
		public UnityEngine.UI.Image ImgStarBubble3;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStarBubble3;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtNowLevel;
		[SerializeField]
		public RectTransform OrderNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnOrder;
		[SerializeField]
		public UnityEngine.UI.Image ImgOrderIcon;
		[SerializeField]
		public UnityEngine.UI.Image ImgOrderTip;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOrderTip;
		[SerializeField]
		public UnityEngine.UI.Image ImgNormalcon;
		[SerializeField]
		public UnityEngine.UI.Image ImgJigsawIcon;
		[SerializeField]
		public UnityEngine.UI.Image ImgHardIcon;
		[SerializeField]
		public UnityEngine.UI.Image ImgOrderUnlock;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOrderUnlock;
		[SerializeField]
		public RectTransform OrderLevelNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgOrderScroll;
		[SerializeField]
		public RectTransform OrderBegin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOrderBegin;
		[SerializeField]
		public RectTransform OrderEnd;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOrderEnd;
		[SerializeField]
		public UnityEngine.UI.Button BtnOrderContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtContinue;
		[SerializeField]
		public UnityEngine.UI.Button BtnSkip;
		[SerializeField]
		public TMPro.TMP_InputField InputLevel;
		[SerializeField]
		public Transform JigsawNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnJigsawMain;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtJigsawTitle;
		[SerializeField]
		public Animator GoGuochang;
		[SerializeField]
		public Transform ChallengeNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnChallengeMain;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtScientist;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtChallengeTitle;
		[SerializeField]
		public Transform SevenNode;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtClear;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtGetStar;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtGetReward;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLimitTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnClickSeven;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtClickContinue;
		[SerializeField]
		public RectTransform BeginNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnSet;
		[SerializeField]
		public UnityEngine.UI.Button BtnAD;
		[SerializeField]
		public UnityEngine.UI.Button BtnLevel;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnChallenge;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtChallenge;
		[SerializeField]
		public UnityEngine.UI.Button BtnShowCake;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtShowCake;
		
		private UIBeginData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TeachNode = null;
			TxtTeach = null;
			ImgTeach = null;
			Step2Node = null;
			Step1Node = null;
			TipNode = null;
			TxtTip = null;
			JigsawChangeNode = null;
			BtnJigsaw = null;
			ImgJigsawUnlock = null;
			TxtJigsawUnlock = null;
			LevelNode = null;
			BtnMain = null;
			BtnRefresh = null;
			BtnChehui = null;
			TxtReturnNum = null;
			BtnTianjia = null;
			TxtAddNum = null;
			BtnRank = null;
			TxtRankNum = null;
			GoStep = null;
			BtnStep = null;
			TxtStep = null;
			StarNode = null;
			ScrollNodeBegin = null;
			ScrollNodeEnd = null;
			ImgScrollBg = null;
			ImgScroll = null;
			ImgStarBubbleMove = null;
			TxtStarBubbleMove = null;
			ImgStarBubble2 = null;
			TxtStarBubble2 = null;
			ImgStarBubble3 = null;
			TxtStarBubble3 = null;
			TxtNowLevel = null;
			OrderNode = null;
			BtnOrder = null;
			ImgOrderIcon = null;
			ImgOrderTip = null;
			TxtOrderTip = null;
			ImgNormalcon = null;
			ImgJigsawIcon = null;
			ImgHardIcon = null;
			ImgOrderUnlock = null;
			TxtOrderUnlock = null;
			OrderLevelNode = null;
			ImgOrderScroll = null;
			OrderBegin = null;
			TxtOrderBegin = null;
			OrderEnd = null;
			TxtOrderEnd = null;
			BtnOrderContinue = null;
			TxtContinue = null;
			BtnSkip = null;
			InputLevel = null;
			JigsawNode = null;
			BtnJigsawMain = null;
			TxtJigsawTitle = null;
			GoGuochang = null;
			ChallengeNode = null;
			BtnChallengeMain = null;
			TxtScientist = null;
			TxtChallengeTitle = null;
			SevenNode = null;
			TxtClear = null;
			TxtGetStar = null;
			TxtGetReward = null;
			TxtLimitTime = null;
			BtnClickSeven = null;
			TxtClickContinue = null;
			BeginNode = null;
			BtnSet = null;
			BtnAD = null;
			BtnLevel = null;
			TxtLevel = null;
			BtnChallenge = null;
			TxtChallenge = null;
			BtnShowCake = null;
			TxtShowCake = null;
			
			mData = null;
		}
		
		public UIBeginData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBeginData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBeginData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
