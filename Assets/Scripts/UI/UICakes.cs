using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using System.Collections;

namespace QFramework.Example
{
	public class UICakesData : UIPanelData
	{
		public bool showUnlock;
	}
	public partial class UICakes : UIPanel, ICanGetUtility
	{
		[SerializeField]
		List<Image> cakes = new List<Image>();
		[SerializeField]
		List<Sprite> unlockCake = new List<Sprite>();
        List<Image> unlockCakes = new List<Image>();
        List<int> idxCakes = new List<int>();
        List<GameObject> cakesGo = new List<GameObject>();
		int unlockIdx = 1;
		int nowPage = 0;
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UICakesData ?? new UICakesData();
			// please add init code here
		}

        private void Start()
        {
			RegisterEvent();
        }

        void RegisterEvent()
		{
            BtnLeft.onClick.AddListener(() =>
            {
                if(nowPage > 0)
				{
					nowPage--;
                }
                CheckArrowBtnShow();
                SetCake(true);
                AudioKit.PlaySound("resources://Audio/btnClick");

            });

            BtnRight.onClick.AddListener(() =>
            {
                if(nowPage < 2)
				{
					nowPage++;
                }
                CheckArrowBtnShow();
                SetCake(true);
                AudioKit.PlaySound("resources://Audio/btnClick");

            });

			BtnReturn.onClick.AddListener(() =>
			{
				UIKit.HidePanel<UICakes>();
				AudioKit.PlaySound("resources://Audio/btnClick");
			});
        }

		void CheckArrowBtnShow()
		{
			BtnLeft.gameObject.SetActive(nowPage > 0);
			BtnRight.gameObject.SetActive(nowPage < 2);
			TxtCake.text = (nowPage + 1) + "/" + 3;
        }

        //private void Update()
        //{
        //    if (Input.GetKeyUp(KeyCode.F4))
        //    {
        //        SetCake();
        //    }
        //}

		int GetUnlockNum(int level = -1)
		{
            if (level == -1)
            {
                level = this.GetUtility<SaveDataUtility>().GetLevelClear();
            }
            if (level >= 110)
            {
                return 22;
            }
            else if (level >= 101)
            {
                return 21;
            }
            else if (level >= 94)
            {
                return 20;
            }
            else if (level >= 86)
            {
                return 19;
            }
            else if (level >= 78)
            {
                return 18;
            }
            else if (level >= 70)
            {
                return 17;
            }
            else if (level >= 62)
            {
                return 16;
            }
            else if (level >= 54)
            {
                return 15;
            }
            else if (level >= 49)
            {
                return 12;
            }
            else if (level >= 41)
            {
                return 11;
            }
            else if (level >= 33)
            {
                return 10;
            }
            else if (level >= 28)
            {
                return 8;
            }
            else if (level >= 20)
            {
                return 7;
            }
            else if (level >= 12)
            {
                return 6;
            }
            else if (level >= 10)
            {
                return 5;
            }
            else if (level >= 5)
            {
                return 4;
            }
            else if (level >= 3)
            {
                return 2;
            }
            else if (level >= 2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        protected override void OnOpen(IUIData uiData = null)
		{
        }

		void SetCake(bool isArrow = false)
		{
            if(isArrow)
            {
                foreach(var cake in cakesGo)
                {
                    Destroy(cake);
                }
                cakesGo.Clear();
                LevelManager.Instance.showUnlock = false;
            }
            int lastUnlock = 0;
            if (LevelManager.Instance.showUnlock)
            {
                lastUnlock = GetUnlockNum(LevelManager.Instance.levelId - 1);
            }

            for (int i = 0; i < cakes.Count; i++)
			{
				var cake = cakes[i];
				int nowCake = 9 * nowPage + i;
				if(nowCake <= unlockIdx)
				{
					cake.gameObject.SetActive(true);
                    if(LevelManager.Instance.showUnlock && nowCake > lastUnlock)
                    {
                        cake.gameObject.SetActive(false);
                        unlockCakes.Add(cake);
                        idxCakes.Add(i);
                    }
					cake.sprite = unlockCake[nowCake];
				}
				else
				{
					cake.gameObject.SetActive(false);
                }
            }

            if (LevelManager.Instance.showUnlock)
            {
                StartCoroutine(ShowAnim());
            }

        }

        IEnumerator ShowAnim()
        {
            BtnReturn.gameObject.SetActive(false);
            for (int i = 0; i < unlockCakes.Count; i++) 
            {
                var cake = unlockCakes[i];
                var go = Instantiate(CakeFlyNode.gameObject, CakeFlyNode.transform.parent);
                cakesGo.Add(go);
                var fly = go.GetComponent<CakeFly>();
                fly.cake.sprite = cake.sprite;
                fly.originCake = cake.gameObject;
                fly.SetPath(idxCakes[i] + 1);
                go.gameObject.SetActive(true);
                yield return new WaitForSeconds(2);
            }
            BtnReturn.gameObject.SetActive(true);

        }

        protected override void OnShow()
        {
            TopOnADManager.Instance.RemoveBannerAd();
            unlockCakes.Clear();
            cakesGo.Clear();
            idxCakes.Clear();
            unlockIdx = GetUnlockNum();
            nowPage = (int)((unlockIdx + 0.9f) / 9);
            ImgTeach.gameObject.SetActive(this.GetUtility<SaveDataUtility>().GetLevelClear() == 2);
            SetCake();
            CheckArrowBtnShow();
        }
		
		protected override void OnHide()
		{
            LevelManager.Instance.showUnlock = false;
        }

        protected override void OnClose()
		{
            LevelManager.Instance.showUnlock = false;
        }
    }
}
