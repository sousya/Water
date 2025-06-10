using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:670f12ca-7b68-4368-899c-d3d706f8389e
	public partial class UIGetCoin
	{
		public const string Name = "UIGetCoin";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.GameObject ImgProcessNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgProcess;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtProcess;
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		
		private UIGetCoinData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			TxtLevel = null;
			TxtCoin = null;
			ImgProcessNode = null;
			ImgProcess = null;
			TxtProcess = null;
			BtnContinue = null;
			
			mData = null;
		}
		
		public UIGetCoinData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGetCoinData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGetCoinData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
