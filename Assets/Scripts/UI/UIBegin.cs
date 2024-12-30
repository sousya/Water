using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIBeginData : UIPanelData
	{
	}
	public partial class UIBegin : UIPanel, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

		public GameObject BeginNode, LevelNode, SceneNode1, SceneNode2, SceneNode3;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBeginData ?? new UIBeginData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
            TxtLevel.font.material.shader = Shader.Find(TxtLevel.font.material.shader.name);

        }

        void BindBtn()
        {
            BtnAddBottle.onClick.RemoveAllListeners();
            BtnRefresh.onClick.RemoveAllListeners();

			BtnRefresh.onClick.AddListener(() =>
			{
				LevelManager.Instance.RefreshLevel();
            });

			BtnAddBottle.onClick.AddListener(() =>
            {
                LevelManager.Instance.AddBottle();
            });

            BtnStart.onClick.AddListener(() =>
            {
                LevelManager.Instance.StartGame(1);
				LevelNode.SetActive(true);
                BeginNode.SetActive(false);
            });
        }

		void RegisterEvent()
		{
			this.RegisterEvent<LevelStartEvent>(e =>
			{
                TxtLevel.text = "Level " + LevelManager.Instance.levelId;
            });
		}

		void SetText()
		{
            TxtLevel.text = "Level " + LevelManager.Instance.levelId;
        }

		void SetScene()
		{
			
		}

        protected override void OnShow()
		{
			BindBtn();
			RegisterEvent();
			SetText();
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
