using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UIBuyItemData : UIPanelData
	{
		public int item;
	}
	public partial class UIBuyItem : UIPanel, ICanGetUtility, ICanSendEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public List<string> names;
		public List<string> descs;
		public List<int> costs;
		public List<Sprite> icons;

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBuyItemData ?? new UIBuyItemData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
            var utility = this.GetUtility<SaveDataUtility>();
            var nowCoin = utility.GetCoinNum();
            var needCoin = costs[mData.item - 1];
            if (nowCoin < needCoin)
            {
				TxtCost.color = Color.red;
            }
            else
			{
                TxtCost.color = Color.white;
            }
				
			ImgItem.sprite = icons[mData.item - 1];
            TxtTitle.text = names[mData.item - 1];
			TxtDesc.text = descs[mData.item - 1];
			TxtCost.text = costs[mData.item - 1].ToString();
            BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
			{
                CloseSelf();
            });
			
            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                //var utility = this.GetUtility<SaveDataUtility>();
                //var nowCoin = utility.GetCoinNum();
                //var needCoin = costs[mData.item - 1];
                if (nowCoin < needCoin)
				{
					return;
				}
				utility.SetCoinNum(nowCoin - needCoin);
				utility.AddItemNum(mData.item);
				this.SendEvent<RefreshItemEvent>();
                CloseSelf();
            });

        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
