using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;

namespace QFramework.Example
{
    public class UIUnlockSceneData : UIPanelData
    {
    }

    public partial class UIUnlockScene : UIPanel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
    {
        public Animator boxAnim;
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIUnlockSceneData ?? new UIUnlockSceneData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
            SetScene();
            BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

            BtnBox.onClick.RemoveAllListeners();
            BtnBox.onClick.AddListener(() =>
            {
                var partNow = this.GetUtility<SaveDataUtility>().GetScenePartRecord();
                if (partNow == 5)
                {
                    StartCoroutine(OpenBox());
                }
                else
                {
                    ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
                }
            });
        }

        void SetScene()
        {
            var sceneNow = this.GetUtility<SaveDataUtility>().GetSceneRecord();
            var partNow = this.GetUtility<SaveDataUtility>().GetScenePartRecord();
            bool.TryParse(this.GetUtility<SaveDataUtility>().GetOverUnLock(), out bool overUnlock);
            if (overUnlock)
            {
                ImgUnlockItem1.Hide();
                ImgUnlockItem2.Hide();
                ImgUnlockItem3.Hide();
                ImgUnlockItem4.Hide();
                ImgUnlockItem5.Hide();
                ImgProgress.fillAmount = 0;
                TxtImgprogress.text = "0 / 5";
                BtnBox.interactable = false;
                return;
            }

            ImgProgress.fillAmount = partNow / 5f;
            TxtImgprogress.text = partNow + " / 5";
            if (partNow < 5)
            {
                ImgUnlockItem1.gameObject.SetActive(partNow + 1 == 1);
                ImgUnlockItem2.gameObject.SetActive(partNow + 1 == 2);
                ImgUnlockItem3.gameObject.SetActive(partNow + 1 == 3);
                ImgUnlockItem4.gameObject.SetActive(partNow + 1 == 4);
                ImgUnlockItem5.gameObject.SetActive(partNow + 1 == 5);
            }
            else
            {
                ImgUnlockItem1.Hide();
                ImgUnlockItem2.Hide();
                ImgUnlockItem3.Hide();
                ImgUnlockItem4.Hide();
                ImgUnlockItem5.Hide();
            }
            /*else
            {
                ImgUnlockItem2.gameObject.SetActive(false);
                ImgUnlockItem3.gameObject.SetActive(false);
                ImgUnlockItem4.gameObject.SetActive(false);
                ImgUnlockItem5.gameObject.SetActive(false);
                if (this.GetUtility<SaveDataUtility>().GetSceneBox() == sceneNow)
                {
                    ImgUnlockItem1.gameObject.SetActive(true);
                    //sceneNow++;//当前只有四个场景
                    sceneNow = sceneNow + 1 > 4 ? 4 : sceneNow + 1;
                    TxtImgprogress.text = 0 + " / 5";
                    ImgProgress.fillAmount = 0;

                }
                else
                {
                    ImgUnlockItem1.gameObject.SetActive(false);
                }
            }*/

            ImgUnlockItem1.SetItem(sceneNow, 1);
            ImgUnlockItem2.SetItem(sceneNow, 2);
            ImgUnlockItem3.SetItem(sceneNow, 3);
            ImgUnlockItem4.SetItem(sceneNow, 4);
            ImgUnlockItem5.SetItem(sceneNow, 5);
        }

        /// <summary>
        /// 打开宝箱
        /// </summary>
        /// <returns></returns>
        IEnumerator OpenBox()
        {
            //避免打断下面逻辑
            BtnClose.interactable = false;
            //增加道具
            this.GetUtility<SaveDataUtility>().AddItemNum(1, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(2, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(3, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(4, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(5, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(6, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(7, 1);
            this.GetUtility<SaveDataUtility>().AddItemNum(8, 1);
            boxAnim.Play("BoxOpen");
            bool overUnlock = CheckOverUnLock(this.GetUtility<SaveDataUtility>().GetSceneRecord(),
                this.GetUtility<SaveDataUtility>().GetScenePartRecord());
            if (!overUnlock)
                this.GetUtility<SaveDataUtility>().SetScenePartRecord(0);

            yield return new WaitForSeconds(1f);
            this.SendEvent<RewardSceneEvent>();
            BtnClose.interactable = true;
            CloseSelf();
        }

        private bool CheckOverUnLock(int sceneNow ,int partNow)
        {
            if (sceneNow == 4 && partNow == 5)
            {
                //Debug.Log("当前解锁进度已满");

                this.GetUtility<SaveDataUtility>().SetOverUnLock(true);
                return true;
            }

            return false;
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
