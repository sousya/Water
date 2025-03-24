using UnityEngine;
using UnityEngine.UI;
using WaterGame.Models;

namespace WaterGame.Controllers
{
    public class BottleWaterCtrl : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image waterImg;
        [SerializeField] private GameObject spineGo;
        [SerializeField] private GameObject broomItemGo;
        [SerializeField] private GameObject createItemGo;
        [SerializeField] private GameObject changeItemGo;
        [SerializeField] private GameObject magnetItemGo;
        [SerializeField] private Text textItem;

        [Header("Animation")]
        [SerializeField] private float fillAnimDuration = 0.5f;
        [SerializeField] private float outAnimDuration = 0.5f;

        private bool _isPlayItemAnim;
        private Color _waterColor;
        private ItemType _currentItemType;

        public bool isPlayItemAnim => _isPlayItemAnim;
        public Image waterImage => waterImg;

        public void SetColorState(ItemType itemType, Color color)
        {
            _currentItemType = itemType;
            _waterColor = color;
            waterImg.color = color;
            UpdateItemVisuals();
        }

        public void SetHide(bool hide, bool noWait = false)
        {
            if (noWait)
            {
                waterImg.fillAmount = hide ? 0 : 1;
            }
            else
            {
                waterImg.DOFillAmount(hide ? 0 : 1, 0.3f);
            }
        }

        public void PlayFillAnim(float duration)
        {
            waterImg.fillAmount = 0;
            waterImg.DOFillAmount(1, duration);
        }

        public void PlayOutAnim(float duration)
        {
            waterImg.DOFillAmount(0, duration);
        }

        public void PlayUseBroom(BottleWaterCtrl target)
        {
            _isPlayItemAnim = true;
            broomItemGo.SetActive(true);
            StartCoroutine(PlayItemAnimation(target));
        }

        public void PlayUseCreate(BottleCtrl bottle, BottleWaterCtrl target)
        {
            _isPlayItemAnim = true;
            createItemGo.SetActive(true);
            StartCoroutine(PlayItemAnimation(target));
        }

        public void PlayUseChange(BottleWaterCtrl target)
        {
            _isPlayItemAnim = true;
            changeItemGo.SetActive(true);
            StartCoroutine(PlayItemAnimation(target));
        }

        public void PlayUseMagnet(BottleWaterCtrl target)
        {
            _isPlayItemAnim = true;
            magnetItemGo.SetActive(true);
            StartCoroutine(PlayItemAnimation(target));
        }

        private void UpdateItemVisuals()
        {
            spineGo.SetActive(_currentItemType == ItemType.UseColor);
            broomItemGo.SetActive(false);
            createItemGo.SetActive(false);
            changeItemGo.SetActive(false);
            magnetItemGo.SetActive(false);
        }

        private System.Collections.IEnumerator PlayItemAnimation(BottleWaterCtrl target)
        {
            yield return new WaitForSeconds(1.5f);
            _isPlayItemAnim = false;
            UpdateItemVisuals();
        }

        public IEnumerator ShowBroomAfter()
        {
            yield return new WaitForSeconds(2.2f);
            gameObject.SetActive(false);
        }

        public IEnumerator ShowThunder(Transform target)
        {
            var thunder = Instantiate(LevelManager.Instance.thunderPrefab, transform.position, Quaternion.identity);
            thunder.transform.SetParent(transform);
            yield return new WaitForSeconds(0.5f);
            thunder.transform.DOMove(target.position, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Destroy(thunder);
        }

        public IEnumerator ChangeShine()
        {
            var img = GetComponent<Image>();
            img.material = LevelManager.Instance.shineMaterial;
            yield return new WaitForSeconds(2.2f);
            img.material = null;
        }
    }
} 