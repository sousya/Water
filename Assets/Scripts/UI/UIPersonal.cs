using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIPersonalData : UIPanelData
	{
	}
	public partial class UIPersonal : UIPanel, ICanRegisterEvent
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIPersonalData ?? new UIPersonalData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			//ע���޸��¼�
			this.RegisterEvent<AvatarEvent>(e =>
			{
                BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true, e.AvatarId);
                ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false, e.AvatarFrameId);
            }).UnRegisterWhenGameObjectDestroyed(this);

            //��ȡ��ʼͷ���ͷ���
            BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true);
			ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false);

            BtnHead.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<UIChooseAvatar>();
			});

			BtnClose.onClick.AddListener(() =>
            {
                UIKit.ClosePanel(this);
            });
        }

        protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            BtnClose.onClick.RemoveAllListeners();
            BtnHead.onClick.RemoveAllListeners();
        }

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
    }
}
