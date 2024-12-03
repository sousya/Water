using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:79f17480-a6d3-40e4-8126-ece5890e5458
	public partial class UIJudge
	{
		public const string Name = "UIJudge";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtJudge;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIJudgeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtJudge = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIJudgeData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIJudgeData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIJudgeData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
