using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:f2ec159f-7780-4ae9-9a90-7076d39b23e9
	public partial class UIBegin
	{
		public const string Name = "UIBegin";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnStepBack;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRefreshNum;
		[SerializeField]
		public UnityEngine.UI.Image BtnAddStepBack;
		[SerializeField]
		public UnityEngine.UI.Button BtnRemoveHide;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRemoveHideNum;
		[SerializeField]
		public UnityEngine.UI.Image BtnAddRemove;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddBottle;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddBottleNum;
		[SerializeField]
		public UnityEngine.UI.Image BtnAddAddBottle;
		[SerializeField]
		public UnityEngine.UI.Button BtnHalfBottle;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddHalfBottleNum;
		[SerializeField]
		public UnityEngine.UI.Image BtnAddHalfBottle;
		[SerializeField]
		public UnityEngine.UI.Button BtnRemoveAll;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRemoveAllNum;
		[SerializeField]
		public UnityEngine.UI.Image BtnAddRemoveBottle;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnMenu;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		[SerializeField]
		public UnityEngine.UI.Image BtnItemBg;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem1;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem1;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem2;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem2;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem3;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem3;
		[SerializeField]
		public UnityEngine.UI.ScrollRect ShopScrollView;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStart;
		[SerializeField]
		public UnityEngine.UI.Button BtnArea;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtArea;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgressBg;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtImgprogress;
		[SerializeField]
		public UnityEngine.UI.Button BtnHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.UI.Button BtnStar;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStar;
		[SerializeField]
		public Transform RewardNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnGetReward;
		[SerializeField]
		public RectTransform Target;
		[SerializeField]
		public RectTransform Begin8;
		[SerializeField]
		public RectTransform Begin7;
		[SerializeField]
		public RectTransform Begin6;
		[SerializeField]
		public RectTransform Begin5;
		[SerializeField]
		public RectTransform Begin4;
		[SerializeField]
		public RectTransform Begin3;
		[SerializeField]
		public RectTransform Begin2;
		[SerializeField]
		public RectTransform Begin1;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem8;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem7;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem6;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem5;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem4;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem3;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem2;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem1;
		[SerializeField]
		public ParticleTargetMoveCtrl RewardCoinFx;
		[SerializeField]
		public UnityEngine.Animator TxtCoinAdd;
		[SerializeField]
		public UnityEngine.UI.Button BtnHead;
		[SerializeField]
		public UnityEngine.UI.Button BtnSelect;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelected;
		[SerializeField]
		public RectTransform BottomMenuBtns;
		
		private UIBeginData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnStepBack = null;
			TxtRefreshNum = null;
			BtnAddStepBack = null;
			BtnRemoveHide = null;
			TxtRemoveHideNum = null;
			BtnAddRemove = null;
			BtnAddBottle = null;
			TxtAddBottleNum = null;
			BtnAddAddBottle = null;
			BtnHalfBottle = null;
			TxtAddHalfBottleNum = null;
			BtnAddHalfBottle = null;
			BtnRemoveAll = null;
			TxtRemoveAllNum = null;
			BtnAddRemoveBottle = null;
			TxtLevel = null;
			BtnMenu = null;
			BtnReturn = null;
			BtnItemBg = null;
			BtnItem1 = null;
			TxtItem1 = null;
			BtnItem2 = null;
			TxtItem2 = null;
			BtnItem3 = null;
			TxtItem3 = null;
			ShopScrollView = null;
			BtnStart = null;
			TxtStart = null;
			BtnArea = null;
			TxtArea = null;
			ImgProgressBg = null;
			ImgProgress = null;
			TxtImgprogress = null;
			BtnHeart = null;
			TxtHeart = null;
			TxtTime = null;
			BtnCoin = null;
			TxtCoin = null;
			BtnStar = null;
			TxtStar = null;
			RewardNode = null;
			BtnGetReward = null;
			Target = null;
			Begin8 = null;
			Begin7 = null;
			Begin6 = null;
			Begin5 = null;
			Begin4 = null;
			Begin3 = null;
			Begin2 = null;
			Begin1 = null;
			ImgItem8 = null;
			ImgItem7 = null;
			ImgItem6 = null;
			ImgItem5 = null;
			ImgItem4 = null;
			ImgItem3 = null;
			ImgItem2 = null;
			ImgItem1 = null;
			RewardCoinFx = null;
			TxtCoinAdd = null;
			BtnHead = null;
			BtnSelect = null;
			ImgSelected = null;
			BottomMenuBtns = null;
			
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
