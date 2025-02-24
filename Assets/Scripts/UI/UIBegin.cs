using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UIBeginData : UIPanelData
	{
	}
	public partial class UIBegin : UIPanel, ICanRegisterEvent, ICanGetUtility
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

		public List<Animator> ButtonAnim;

		public GameObject BeginNode, LevelNode, SceneNode1, SceneNode2, SceneNode3, SceneNode4, SceneNode5;
        public ScenePartCtrl ScenePart1, ScenePart2, ScenePart3, ScenePart4, ScenePart5;
        public ParticleTargetMoveCtrl coinFx;


		int nowButton = 2;

        public List<int> checkScene = new List<int>();
        public List<int> needScene1 = new List<int>();
        public List<int> needScene2 = new List<int>();
        public List<int> needScene3 = new List<int>();
        public List<int> needScene4 = new List<int>();
        public List<int> needScene5 = new List<int>();

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
                if (!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
                {
                    LevelManager.Instance.ReturnLast();
                }
            });

			BtnAddBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
                {
                    LevelManager.Instance.AddBottle(false);
                }
            });

			BtnAddHalfBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
                {
                    LevelManager.Instance.AddBottle(true);
                }
            });

            BtnAddRemoveAll.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
                {
                    LevelManager.Instance.RemoveAll();
                }
            });

            BtnRemoveHide.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
                {
                    LevelManager.Instance.RemoveHide();
                }
            });

            BtnStart.onClick.AddListener(() =>
            {
                LevelManager.Instance.StartGame(this.GetUtility<SaveDataUtility>().GetLevelClear());
				LevelNode.SetActive(true);
                BeginNode.SetActive(false);
            });

			BeginMenuButton1.onClick.AddListener(() =>
			{
				nowButton = 1;
                CheckBeginMenuButton();
            });

            BeginMenuButton2.onClick.AddListener(() =>
			{
				nowButton = 2;
                CheckBeginMenuButton();
            });

            BeginMenuButton3.onClick.AddListener(() =>
			{
				nowButton = 3;
                CheckBeginMenuButton();
            });

            BtnReturn.onClick.AddListener(() =>
            {
                LevelNode.SetActive(false);
                BeginNode.SetActive(true);
                SetScene();
                CheckBeginMenuButton();
                this.GetUtility<SaveDataUtility>().CostVitality();
            });

            BtnHeart.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIMoreLife>();
            });
        }

		void CheckBeginMenuButton()
		{
			switch(nowButton)
            {
                case 1:
                    ButtonAnim[0].Play("BeginMenuButtonSelect");
                    ButtonAnim[1].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[2].Play("BeginMenuButtonUnSelect");
                    break;
                case 2:
                    ButtonAnim[0].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[1].Play("BeginMenuButtonSelect");
                    ButtonAnim[2].Play("BeginMenuButtonUnSelect");
                    break;
                case 3:
                    ButtonAnim[0].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[1].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[2].Play("BeginMenuButtonSelect");
                    break;
            }
        }

		void RegisterEvent()
		{
			this.RegisterEvent<LevelStartEvent>(e =>
			{
                TxtLevel.text = "Level " + LevelManager.Instance.levelId;
            });

            this.RegisterEvent<LevelClearEvent>(e =>
            {
                SetScene();
                ReturnBegin();
            });

            this.RegisterEvent<CoinChangeEvent>(e =>
            {
                SetCoin();
            });

            this.RegisterEvent<VitalityChangeEvent>(e =>
            {
                SetVitality();
            });
        }

        void SetCoin()
        {
            TxtCoin.text = this.GetUtility<SaveDataUtility>().GetCoinNum().ToString();
        }

        void SetVitality()
        {
            TxtHeart.text = this.GetUtility<SaveDataUtility>().GetVitalityNum().ToString();
        }

		void SetText()
		{
            TxtLevel.text = "Level " + LevelManager.Instance.levelId;
        }

        void ReturnBegin()
        {

            BeginNode.SetActive(true);
            LevelNode.SetActive(false);
            coinFx.Play(10);

        }

        void SetScene()
		{
			var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();

            var useScene = 0;
            for (int i = 0; i < checkScene.Count; i++)
            {
                if (levelNow >= checkScene[i])
                {
                    useScene = i + 1;
                }
            }

            switch (useScene)
            {
                case 1:
                    SceneNode1.SetActive(true);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(false);
                    SceneNode5.SetActive(false);
                    break;
                case 2:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(true);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(false);
                    SceneNode5.SetActive(false);
                    break;
                case 3:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(true);
                    SceneNode4.SetActive(false);
                    SceneNode5.SetActive(false);
                    break;
                case 4:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(true);
                    SceneNode5.SetActive(false);
                    break;
                case 5:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(false);
                    SceneNode5.SetActive(true);
                    break;
            }

            SetScenePart(useScene, checkScene[useScene - 1], levelNow);

        }

        void SetScenePart(int scene, int baseStar, int nowStar)
        {
            switch (scene)
            {
                case 1:
                    ScenePart1.ScenePart1.SetActive(baseStar + needScene1[0] <= nowStar);
                    ScenePart1.ScenePart2.SetActive(baseStar + needScene1[0] + needScene1[1] <= nowStar);
                    ScenePart1.ScenePart3.SetActive(baseStar + needScene1[0] + needScene1[1] + needScene1[2] <= nowStar);
                    ScenePart1.ScenePart4.SetActive(baseStar + needScene1[0] + needScene1[1] + needScene1[2] + needScene1[3] <= nowStar);
                    ScenePart1.ScenePart5.SetActive(baseStar + needScene1[0] + needScene1[1] + needScene1[2] + needScene2[3] + needScene1[4] <= nowStar);
                    break;
                case 2:
                    ScenePart2.ScenePart1.SetActive(baseStar + needScene2[0] <= nowStar);
                    ScenePart2.ScenePart2.SetActive(baseStar + needScene2[0] + needScene2[1] <= nowStar);
                    ScenePart2.ScenePart3.SetActive(baseStar + needScene2[0] + needScene2[1] + needScene2[2] <= nowStar);
                    ScenePart2.ScenePart4.SetActive(baseStar + needScene2[0] + needScene2[1] + needScene2[2] + needScene2[3] <= nowStar);
                    ScenePart2.ScenePart5.SetActive(baseStar + needScene2[0] + needScene2[1] + needScene2[2] + needScene2[3] + needScene2[4] <= nowStar);
                    break;
                case 3:
                    ScenePart3.ScenePart1.SetActive(baseStar + needScene3[0] <= nowStar);
                    ScenePart3.ScenePart2.SetActive(baseStar + needScene3[0] + needScene3[1] <= nowStar);
                    ScenePart3.ScenePart3.SetActive(baseStar + needScene3[0] + needScene3[1] + needScene3[2] <= nowStar);
                    ScenePart3.ScenePart4.SetActive(baseStar + needScene3[0] + needScene3[1] + needScene3[2] + needScene3[3] <= nowStar);
                    ScenePart3.ScenePart5.SetActive(baseStar + needScene3[0] + needScene3[1] + needScene3[2] + needScene3[3] + needScene3[4] <= nowStar);
                    break;
                case 4:
                    ScenePart4.ScenePart1.SetActive(baseStar + needScene4[0] <= nowStar);
                    ScenePart4.ScenePart2.SetActive(baseStar + needScene4[0] + needScene4[1] <= nowStar);
                    ScenePart4.ScenePart3.SetActive(baseStar + needScene4[0] + needScene4[1] + needScene4[2] <= nowStar);
                    ScenePart4.ScenePart4.SetActive(baseStar + needScene4[0] + needScene4[1] + needScene4[2] + needScene4[3] <= nowStar);
                    ScenePart4.ScenePart5.SetActive(baseStar + needScene4[0] + needScene4[1] + needScene4[2] + needScene4[3] + needScene4[4] <= nowStar);
                    break;
                case 5:
                    ScenePart5.ScenePart1.SetActive(baseStar + needScene5[0] <= nowStar);
                    ScenePart5.ScenePart2.SetActive(baseStar + needScene5[0] + needScene5[1] <= nowStar);
                    ScenePart5.ScenePart3.SetActive(baseStar + needScene5[0] + needScene5[1] + needScene5[2] <= nowStar);
                    ScenePart5.ScenePart4.SetActive(baseStar + needScene5[0] + needScene5[1] + needScene5[2] + needScene5[3] <= nowStar);
                    ScenePart5.ScenePart5.SetActive(baseStar + needScene5[0] + needScene5[1] + needScene5[2] + needScene5[3] + needScene5[4] <= nowStar);
                    break;

            }
        }
        protected override void OnShow()
		{
			BindBtn();
			RegisterEvent();
			SetText();
            SetCoin();
            SetVitality();
            ButtonAnim[0].Play("BeginMenuButtonUnSelect");
            ButtonAnim[1].Play("BeginMenuButtonSelect");
            ButtonAnim[2].Play("BeginMenuButtonUnSelect");

            var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
            if (levelNow <= 5)
            {
                LevelNode.SetActive(true);
                BeginNode.SetActive(false);
            }
            SetScene();
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
