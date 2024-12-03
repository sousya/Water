using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:b5ae7542-fba9-4a70-ae78-637610df702b
	public partial class UITips
	{
		public const string Name = "UITips";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRank;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOrder;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtJigsaw;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtChallenge;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtExplore;
		[SerializeField]
		public UnityEngine.UI.Button BtnBomb;
		[SerializeField]
		public UnityEngine.UI.Image ImgBomb;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtBomb;
		[SerializeField]
		public UnityEngine.UI.Button BtnLock;
		[SerializeField]
		public UnityEngine.UI.Image ImgLock;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLock;
		[SerializeField]
		public UnityEngine.UI.Button BtnConnect;
		[SerializeField]
		public UnityEngine.UI.Image ImgConnect;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtConnect;
		[SerializeField]
		public UnityEngine.UI.Button BtnFixed;
		[SerializeField]
		public UnityEngine.UI.Image ImgFixed;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtFixed;
		[SerializeField]
		public UnityEngine.UI.Button BtnStepHide;
		[SerializeField]
		public UnityEngine.UI.Image ImgStepHide;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStepHide;
		[SerializeField]
		public UnityEngine.UI.Button BtnAllHide;
		[SerializeField]
		public UnityEngine.UI.Image ImgAllHide;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAllHide;
		[SerializeField]
		public UnityEngine.UI.Button BtnThreeEmpty;
		[SerializeField]
		public UnityEngine.UI.Image ImgThreeEmpty;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtThreeEmpty;
		[SerializeField]
		public UnityEngine.UI.Button BtnOneColor;
		[SerializeField]
		public UnityEngine.UI.Image ImgOneColor;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtOneColor;
		[SerializeField]
		public UnityEngine.UI.Button BtnMoreLayers;
		[SerializeField]
		public UnityEngine.UI.Image ImgMoreLayers;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtMoreLayers;
		[SerializeField]
		public UnityEngine.UI.Button BtnCountDown;
		[SerializeField]
		public UnityEngine.UI.Image ImgCountDown;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCountDown;
		[SerializeField]
		public UnityEngine.UI.Button BtnLeftLevel;
		[SerializeField]
		public UnityEngine.UI.Image ImgLeftLevel;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLeftLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnCombine;
		[SerializeField]
		public UnityEngine.UI.Image ImgCombine;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCombine;
		[SerializeField]
		public UnityEngine.UI.Button BtnExplore;
		[SerializeField]
		public UnityEngine.UI.Image ImgExplore;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtExploring;
		[SerializeField]
		public UnityEngine.UI.Image ImgShow;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtContent;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtEvent;
		
		private UITipsData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtRank = null;
			TxtOrder = null;
			TxtJigsaw = null;
			TxtChallenge = null;
			TxtExplore = null;
			BtnBomb = null;
			ImgBomb = null;
			TxtBomb = null;
			BtnLock = null;
			ImgLock = null;
			TxtLock = null;
			BtnConnect = null;
			ImgConnect = null;
			TxtConnect = null;
			BtnFixed = null;
			ImgFixed = null;
			TxtFixed = null;
			BtnStepHide = null;
			ImgStepHide = null;
			TxtStepHide = null;
			BtnAllHide = null;
			ImgAllHide = null;
			TxtAllHide = null;
			BtnThreeEmpty = null;
			ImgThreeEmpty = null;
			TxtThreeEmpty = null;
			BtnOneColor = null;
			ImgOneColor = null;
			TxtOneColor = null;
			BtnMoreLayers = null;
			ImgMoreLayers = null;
			TxtMoreLayers = null;
			BtnCountDown = null;
			ImgCountDown = null;
			TxtCountDown = null;
			BtnLeftLevel = null;
			ImgLeftLevel = null;
			TxtLeftLevel = null;
			BtnCombine = null;
			ImgCombine = null;
			TxtCombine = null;
			BtnExplore = null;
			ImgExplore = null;
			TxtExploring = null;
			ImgShow = null;
			TxtContent = null;
			BtnReturn = null;
			TxtEvent = null;
			
			mData = null;
		}
		
		public UITipsData Data
		{
			get
			{
				return mData;
			}
		}
		
		UITipsData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UITipsData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
