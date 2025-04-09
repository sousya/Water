using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:aab1914b-e89e-4d12-8435-15ff5a21c145
	public partial class UILessStar
	{
		public const string Name = "UILessStar";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		
		private UILessStarData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			BtnStart = null;
			
			mData = null;
		}
		
		public UILessStarData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILessStarData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILessStarData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
