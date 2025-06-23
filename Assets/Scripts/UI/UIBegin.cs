using UnityEngine;
using UnityEngine.UI;
using QFramework;
using GameDefine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System;
using TMPro;

namespace QFramework.Example
{
    public class UIBeginData : UIPanelData
    {
    }
    public partial class UIBegin : UIPanel, ICanRegisterEvent, ICanGetUtility, ICanGetModel
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public GameObject[] SceneNodes;
        public ScenePartCtrl[] scenePartCtrls;

        public RectTransform[] ItemBeginPos;
        public Image[] ImgItems;

        public ParticleTargetMoveCtrl coinFx, starFx;

        #region BottomMenuSetting
        [SerializeField] private List<Button> bottomMenuBtns;
        [SerializeField] private List<RectTransform> bottomMenuRect;
        [SerializeField] private List<GameObject> Panels;
        [SerializeField] private RectTransform selectedImg;
        private GameObject HomeNode => Panels[2];
        private int nowButton = 2;
        private readonly Vector2 SELECTED = new Vector2(256, 200);  // ѡ�зŴ�Ĵ�С
        private readonly Vector2 NSELECTED = new Vector2(206, 200); // δѡ�еĴ�С
        private readonly float minScaleValue = 0.5f;                // ��ť����Сֵ(����С��Ŵ�)
        private readonly float maxScaleValue = 1.2f;                // ��ť�ķŴ�ֵ
        private readonly float targetPosY = 80f;                    // ��ť����̧��ĸ߶�
        private readonly float initPosY = 15f;                      // ��ť�ĳ�ʼλ��
        #endregion

        private TextMeshProUGUI mTxtCoinAdd;
        private StageModel stageModel;
        private SaveDataUtility saveData;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIBeginData ?? new UIBeginData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            //���ģʽ�£�AssetBundle ������Դ����Ҫ��������
            //TxtImgprogress.font.material.shader = Shader.Find(TxtImgprogress.font.material.shader.name);
            stageModel = this.GetModel<StageModel>();
            saveData = this.GetUtility<SaveDataUtility>();
            LevelManager.Instance.InitBottle();
            mTxtCoinAdd = TxtCoinAdd.GetComponent<TextMeshProUGUI>();
           

            if (saveData.GetLevelClear() <= 5)
            {
                BottomMenuBtns.Hide();
                HomeNode.Hide();
            }

            else if (saveData.GetLevelClear() > GameConst.WIN_STREAK_BEGIN_LEVEL)
            {
                PotionActivity();
            }
        }

        protected override void OnShow()
        {
            BindBtn();
            RegisterEvent();
            SetAvatar();
            SetVitality();
            SetCoin();
            SetStar();
            SetScene();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        private void Update()
        {
            if (HealthManager.Instance.UnLimitHp || !HealthManager.Instance.IsMaxHp)
            {
                TxtTime.text = HealthManager.Instance.UnLimitHp ?
                    HealthManager.Instance.UnLimitHpTimeStr :
                    HealthManager.Instance.RecoverTimerStr;
            }
        }

        //��ť����
        void BindBtn()
        {
            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBeginSelect>();
            });

            BtnHeart.onClick.RemoveAllListeners();
            BtnHeart.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIMoreLife>();
            });

            BtnArea.onClick.RemoveAllListeners();
            BtnArea.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIUnlockScene>();
            });

            BtnGetReward.onClick.RemoveAllListeners();
            //��ȡ�꽱���ص� 
            BtnGetReward.onClick.AddListener(() =>
            {
                SetScene();
                StartCoroutine(FlyReward());
            });

            BtnHead.onClick.RemoveAllListeners();
            BtnHead.onClick.AddListener(() =>
            {
                UIKit.OpenPanel("UIPersonal");
            });

            BtnCoin.onClick.RemoveAllListeners();
            BtnCoin.onClick.AddListener(() =>
            {
                //��ת�̵�
                InitBeginMenuButton(0);
            });

            //�ײ�����ť����
            foreach (var btn in bottomMenuBtns)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                   int index = bottomMenuBtns.IndexOf(btn);
                   //�л�����
                   ChangePanel(index);
                   if (nowButton != index)
                   {
                       for (int i = 0; i < bottomMenuRect.Count; i++)
                       {
                           var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                           if (i == index)
                           {
                               //����ѡ��Ч��
                               rt.localScale = new Vector3(minScaleValue, minScaleValue, minScaleValue);
                               rt.DOScale(new Vector3(maxScaleValue, maxScaleValue, 1), 0.1f);
                               rt.DOLocalMoveY(targetPosY, 0.1f);
                               bottomMenuRect[index].sizeDelta = SELECTED;
                           }
                           else
                           {
                               //����δѡ��Ч��
                               rt.DOScale(Vector3.one, 0.2f);
                               rt.DOLocalMoveY(initPosY, 0.2f);
                               bottomMenuRect[i].sizeDelta = NSELECTED;
                           }
                       }
                       //�ȴ�һ֡
                       ActionKit.DelayFrame(1, () =>
                       {
                           //ͬ����ť����λ��(�������ð�ť�µ�������ʾ)
                           for (int i = 0; i < bottomMenuBtns.Count; i++)
                           {
                               var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                               rt.DOLocalMoveX(bottomMenuRect[i].localPosition.x, 0.2f);
                           }
                           //���»�����
                           selectedImg.DOMove(bottomMenuRect[index].position, 0.1f);
                           nowButton = index;
                       }).Start(this);
                   }
                });
            }
        }

        //�¼�ע��
        void RegisterEvent()
        {
            //ʤ������=��������ҳ�¼�
            this.RegisterEvent<LevelClearEvent>(e =>
            {
                BottomMenuBtns.Show();
                HomeNode.Show();
                StartCoroutine(ShowFx());

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<ReturnMainEvent>(e =>
            {
                BottomMenuBtns.Show();
                HomeNode.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<CoinChangeEvent>(e =>
            {
                SetCoin(e.coin);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<VitalityChangeEvent>(e =>
            {
                SetVitality();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<UnlockSceneEvent>(e =>
            {
                SetScene();
                ShowFx(e.scene, e.part);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<RewardSceneEvent>(e =>
            {
                ShowReward();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<GameStartEvent>(e =>
            {
                UIKit.OpenPanel<UIGameNode>();
                LevelManager.Instance.StartGame(saveData.GetLevelClear());
                BottomMenuBtns.Hide();
                HomeNode.Hide();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<AvatarEvent>(e =>
            {
                BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true, e.AvatarId);
                ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false, e.AvatarFrameId);
            }).UnRegisterWhenGameObjectDestroyed(this);

            StringEventSystem.Global.Register("OpenShopPanel", () =>
            {
                InitBeginMenuButton(0);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            StringEventSystem.Global.Register("StartPotionActivity", () =>
            {
                PotionActivity();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #region �ײ��˵�����ť�л�

        /// <summary>
        /// �˵���ť����л�����
        /// </summary>
        /// <param name="index"></param>
        void ChangePanel(int index)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                if (i == index)
                    Panels[i].Show();
                else
                    Panels[i].Hide();
            }
        }

        /// <summary>
        /// ��ʾ����ʼ���ײ��˵���ť
        /// </summary>
        void InitBeginMenuButton(int index = -1)
        {
            BottomMenuBtns.Show();
            //�вδ��룬��ʼ��ť���(�л���Ӧ����)
            if (index > -1)
                bottomMenuBtns[index].onClick.Invoke();
        }

        #endregion

        private void SetAvatar()
        {
            BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true);
            ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false);
        }

        /// <summary>
        /// ���½������
        /// </summary>
        /// <param name="num"></param>
        void SetCoin(int num = 0)
        {
            if (num == 0)
                num = CoinManager.Instance.Coin;

            TxtCoin.text = num.ToString();
        }

        /// <summary>
        /// ������������
        /// </summary>
        void SetStar()
        {
            var nowStar = saveData.GetLevelClear() - 1;
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            //���Լ�¼һ����ʹ����������
            var useStar = LevelManager.Instance.GetUnlockNeedStar(sceneNow, partNow);
            TxtStar.text = (nowStar - useStar).ToString();

            //Debug.Log("������������");
        }

        /// <summary>
        /// ��������
        /// </summary>
        void SetVitality()
        {
            TxtHeart.text = HealthManager.Instance.UnLimitHp ? "��" : HealthManager.Instance.NowHp.ToString();

            if (!HealthManager.Instance.UnLimitHp && HealthManager.Instance.IsMaxHp)
                TxtTime.text = "FULL";
        }

        /// <summary>
        /// ��Һ����ǵķ�������Ч��
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowFx()
        {
            mTxtCoinAdd.text = $"+{GameConst.WIN_COINS * stageModel.GoldCoinsMultiple}";
            TxtCoinAdd.Play("TxtUp");
            coinFx.Play(10);
            yield return new WaitForSeconds(0.5f);
            CoinManager.Instance.AddCoin((int)(GameConst.WIN_COINS * stageModel.GoldCoinsMultiple));

            starFx.Play(10);
            yield return new WaitForSeconds(0.5f);
            SetStar();
        }

        /// <summary>
        /// ������ҳ���������Ͳ���UI
        /// </summary>
        void SetScene()
        {
            SetStar();
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();

            TxtArea.text = "Area " + sceneNow;

            //�������(�������һ������)
            if (saveData.GetOverUnLock())
            {
                ImgProgress.fillAmount = 0;
                TxtImgprogress.text = 0 + " / 5";

                var _scene = LevelManager.Instance.SceneUnLockSOs.Count() - 1;
                SceneNodes[_scene].Show();
                SetScenePart(_scene, partNow);
                return;
            }

            //δȫ������
            for (int i = 0; i < SceneNodes.Length; i++)
            {
                //������1��ʼ����
                if ((i + 1) == sceneNow)
                    SceneNodes[i].Show();
                else
                    SceneNodes[i].Hide();
            }

            ImgProgress.fillAmount = partNow / 5f;
            TxtImgprogress.text = partNow + " / 5";

            SetScenePart(sceneNow, partNow);
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="partNow"></param>
        void SetScenePart(int scene, int partNow)
        {
            //������1��ʼ����
            int _index = scene - 1;
            if (_index >= 0 && _index < scenePartCtrls.Length)
            {
                for (int j = 0; j < scenePartCtrls[_index].SceneParts.Length; j++)
                {
                    scenePartCtrls[_index].SceneParts[j].SetActive(partNow > j);
                }
            }
        }

        /// <summary>
        /// ������������������Ч
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="num"></param>
        void ShowFx(int scene, int num)
        {
            //������1��ʼ����
            int _index = scene - 1;
            if (_index >= 0 && _index < scenePartCtrls.Length)
            {
                StartCoroutine(scenePartCtrls[_index].ShowUnlock(num));
            }
        }

        /// <summary>
        /// �����������佱��
        /// </summary>
        void ShowReward()
        {
            RewardNode.gameObject.SetActive(true);

            foreach (var item in ImgItems)
            {
                item.Show();
            }
        }

        /// <summary>
        /// ���߷���Ч��(��ɻص�)
        /// </summary>
        /// <returns></returns>
        IEnumerator FlyReward()
        {
            RewardNode.gameObject.SetActive(false);

            for (int i = 0; i < ImgItems.Length; i++)
            {
                int index = i;
                ImgItems[index].transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        ImgItems[index].Hide();
                        ImgItems[index].transform.position = ItemBeginPos[index].transform.position;
                    });

                yield return new WaitForSeconds(0.2f);
            }

            RewardCoinFx.Play(10);
            yield return new WaitForSeconds(1.5f);
            CoinManager.Instance.AddCoin(200);
        }

        /// <summary>
        /// ��ʤ�
        /// </summary>
        private void PotionActivity()
        {
            //Debug.Log("ʵ���");
            //CountDownTimerManager.Instance.ResetTimer(GameConst.POTION_ACTIVITY_SIGN, 10);
            CountDownTimerManager.Instance.StartTimer(GameConst.POTION_ACTIVITY_SIGN, 1440f);

            if (!CountDownTimerManager.Instance.IsTimerFinished(GameConst.POTION_ACTIVITY_SIGN))
            {
                var potionNode = Resources.Load("Prefab/PotionActivityNode");
                var node = Instantiate(potionNode, HomeNode.transform);
                //Debug.Log("ʣ��ʱ����" + CountDownTimerManager.Instance.GetRemainingTimeText(GameConst.POTION_ACTIVITY_SIGN));
            }
        }
    }
}
