using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;
using GameDefine;
using UnityEditor.Rendering;

namespace QFramework.Example
{
    public class UIUnlockSceneData : UIPanelData
    {
    }

    public partial class UIUnlockScene : UIPanel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent, ICanGetModel
    {
        [SerializeField] private UnlockItemCtrl UnlockItem;
        private StageModel stageModel;
        private SaveDataUtility saveData;

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
            stageModel = this.GetModel<StageModel>();
            saveData = this.GetUtility<SaveDataUtility>();

            SetScene();
            BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

            BtnBox.onClick.RemoveAllListeners();
            BtnBox.onClick.AddListener(() =>
            {
                if (stageModel.SceneBoxUnlock)
                    StartCoroutine(OpenBox());
                else
                    ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
            });
        }

        void SetScene()
        {
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            var overUnlock = saveData.GetOverUnLock();
            //Debug.Log("是否未开启：" + stageModel.SceneBoxUnlock);
            //Debug.Log("是否完成解锁：" + overUnlock);

            // 全部场景解锁完且宝箱已开，停留在最后一个场景
            if (!stageModel.SceneBoxUnlock && overUnlock)
            {
                UnlockItem.Hide();
                ImgProgress.fillAmount = 0;
                TxtImgprogress.text = "0 / 5";
                return;
            }

            ImgProgress.fillAmount = partNow / 5f;
            TxtImgprogress.text = partNow + " / 5";

            // 传递需解锁的场景和建筑索引
            if (partNow < GameConst.SCENE_PART_COUNT)
            {
                UnlockItem.Show();
                UnlockItem.SetItem(sceneNow, partNow + 1);
            }
            else
                UnlockItem.Hide();
        }

        /// <summary>
        /// 打开宝箱
        /// </summary>
        /// <returns></returns>
        IEnumerator OpenBox()
        {
            //避免打断下面逻辑
            BtnClose.interactable = false;
            BtnBox.interactable = false;
            stageModel.SceneBoxUnlock = false; 

            //增加道具
            for (int i = 1; i <= GameDefine.GameConst.ITEM_COUNT; i++)
            {
                stageModel.AddItem(i, 1);
            }
            
            boxAnim.Play("BoxOpen");

            bool overUnlock = saveData.GetOverUnLock();
            if (!overUnlock)
                saveData.SetScenePartRecord(0);
            saveData.SetSceneRecord(saveData.GetSceneRecord() + 1);


            yield return new WaitForSeconds(1f);
            this.SendEvent<RewardSceneEvent>();
            BtnClose.interactable = true;
            BtnBox.interactable = true;
            CloseSelf();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
