using UnityEngine;
using WaterGame.Models;
using WaterGame.Services;
using WaterGame.Events;

namespace WaterGame.Controllers
{
    public class BottleController : MonoBehaviour, IBottleController
    {
        private BottleState _state;
        private AnimationService _animationService;
        private ResourceService _resourceService;

        [Header("UI Components")]
        [SerializeField] private Transform spineGo;
        [SerializeField] private Transform modelGo;
        [SerializeField] private Transform leftMovePlace;
        [SerializeField] private Transform freezeGo;
        [SerializeField] private Animator bottleAnim;
        [SerializeField] private Animator fillWaterGoAnim;
        [SerializeField] private SkeletonGraphic spine;
        [SerializeField] private SkeletonGraphic finishSpine;
        [SerializeField] private SkeletonGraphic freezeSpine;
        [SerializeField] private Image imgWaterTop;
        [SerializeField] private Image imgWaterDown;
        [SerializeField] private Image imgLimit;
        [SerializeField] private GameObject finishGo;
        [SerializeField] private GameObject waterTopSurface;

        [Header("Water Controls")]
        [SerializeField] private List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
        [SerializeField] private List<Transform> spineNode = new List<Transform>();
        [SerializeField] private List<Transform> waterNode = new List<Transform>();

        private Vector3[] _waterRotations = new Vector3[4];
        private BottleRenderUpdate _bottleRenderUpdate;

        private void Awake()
        {
            _animationService = ServiceLocator.Get<AnimationService>();
            _resourceService = ServiceLocator.Get<ResourceService>();
        }

        public void Initialize(BottleState state)
        {
            _state = state;
            InitializeVisuals();
            CalculateWaterRotations();
        }

        private void CalculateWaterRotations()
        {
            var bottleCenterPos = transform.position;
            var sinEdge = Mathf.Abs(waterTopSurface.transform.position.x - bottleCenterPos.x);
            _bottleRenderUpdate = bottleAnim.GetComponent<BottleRenderUpdate>();

            var waterRenderUpdaters = _bottleRenderUpdate.GetComponentsInChildren<WaterRenderUpdater>();
            for (int i = waterRenderUpdaters.Length - 1; i >= 1; i--)
            {
                var position = waterRenderUpdaters[i].waterSurface[0].position;
                var cosEdge = Mathf.Abs(waterTopSurface.transform.position.y - position.y);
                _waterRotations[i] = GetBottleRotation(sinEdge, cosEdge);
            }
            _waterRotations[0] = new Vector3(0, 0, -120);
        }

        private Vector3 GetBottleRotation(float sinEdge, float cosEdge)
        {
            float angle = Mathf.Atan(sinEdge / cosEdge);
            angle = Mathf.PI / 2 - angle;
            return new Vector3(0, 0, -angle * Mathf.Rad2Deg);
        }

        public void Select()
        {
            if (!CanBeSelected()) return;

            _state.IsSelected = true;
            modelGo.transform.localPosition += Vector3.up * 100;
            EventManager.Trigger(GameEvents.OnBottleSelected, this);
        }

        public void Deselect()
        {
            _state.IsSelected = false;
            modelGo.transform.localPosition = Vector3.zero;
        }

        private bool CanBeSelected()
        {
            return !_state.IsFrozen && !_state.IsHidden && !_state.IsClearHidden && 
                   !_state.IsNearHidden && !_state.IsFinished && !_state.IsEmpty;
        }

        public bool CanMoveWater(IBottleController target)
        {
            if (_state.IsEmpty || target.IsFull()) return false;
            if (_state.IsFrozen || _state.IsHidden || _state.IsClearHidden || _state.IsNearHidden) return false;
            if (_state.IsFinished) return false;

            var topWater = GetTopWater();
            if (topWater == null) return false;

            if (target.GetTopWater() != null && target.GetTopWater().Color != topWater.Color)
            {
                return false;
            }

            return true;
        }

        public void MoveWater(IBottleController target)
        {
            if (!CanMoveWater(target)) return;

            var moveAmount = CalculateMoveAmount(target);
            ExecuteWaterMove(target, moveAmount);
        }

        private int CalculateMoveAmount(IBottleController target)
        {
            int availableSpace = target.GetMaxCapacity() - target.GetWaterCount();
            int sameColorCount = CountSameColorWaters();
            return Mathf.Min(availableSpace, sameColorCount);
        }

        private int CountSameColorWaters()
        {
            if (_state.IsEmpty) return 0;

            int count = 1;
            var topColor = GetTopWater().Color;

            for (int i = _state.TopIndex - 1; i >= 0; i--)
            {
                if (_state.Waters[i].Color == topColor && _state.Waters[i].Item != WaterItem.Ice)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private void ExecuteWaterMove(IBottleController target, int moveAmount)
        {
            var topWater = GetTopWater();
            PlayMoveAnimation(target, moveAmount, topWater.Color);

            for (int i = 0; i < moveAmount; i++)
            {
                TransferWater(target);
            }

            Deselect();
            target.PlayFillAnimation(moveAmount, topWater.Color);
        }

        private void PlayMoveAnimation(IBottleController target, int moveAmount, int color)
        {
            _animationService.PlayBottleRotateAnimation(
                modelGo,
                _waterRotations[_state.Waters.Count - moveAmount],
                0.46f,
                () => {
                    _animationService.PlayWaterMoveAnimation(
                        GetWaterTransform(),
                        target.GetWaterTransform(),
                        0.5f
                    );
                }
            );
        }

        private void TransferWater(IBottleController target)
        {
            var topWater = GetTopWater();
            target.AddWater(topWater);
            RemoveTopWater();
        }

        public void AddWater(Water water)
        {
            _state.Waters.Add(water);
            UpdateVisuals();
        }

        public void RemoveTopWater()
        {
            if (!_state.IsEmpty)
            {
                _state.Waters.RemoveAt(_state.TopIndex);
                UpdateVisuals();
            }
        }

        public Water GetTopWater()
        {
            return _state.TopWater;
        }

        public bool IsEmpty()
        {
            return _state.IsEmpty;
        }

        public bool IsFull()
        {
            return _state.IsFull;
        }

        public bool IsFrozen()
        {
            return _state.IsFrozen;
        }

        public bool IsHidden()
        {
            return _state.IsHidden;
        }

        public bool IsClearHidden()
        {
            return _state.IsClearHidden;
        }

        public bool IsNearHidden()
        {
            return _state.IsNearHidden;
        }

        public bool IsFinished()
        {
            return _state.IsFinished;
        }

        public Transform GetWaterTransform()
        {
            return waterNode[_state.TopIndex];
        }

        public Transform GetBottleTransform()
        {
            return transform;
        }

        public int GetBottleIndex()
        {
            return _state.BottleIndex;
        }

        public void SetFrozen(bool frozen)
        {
            _state.IsFrozen = frozen;
            freezeGo.gameObject.SetActive(frozen);
            if (frozen)
            {
                freezeSpine.AnimationState.SetAnimation(0, "idle", false);
            }
        }

        public void SetHidden(bool hidden)
        {
            _state.IsHidden = hidden;
            UpdateVisuals();
        }

        public void SetClearHidden(bool clearHidden)
        {
            _state.IsClearHidden = clearHidden;
            UpdateVisuals();
        }

        public void SetNearHidden(bool nearHidden)
        {
            _state.IsNearHidden = nearHidden;
            UpdateVisuals();
        }

        public void SetFinished(bool finished)
        {
            _state.IsFinished = finished;
            finishGo.SetActive(finished);
            if (finished)
            {
                finishSpine.AnimationState.SetAnimation(0, "animation", false);
            }
        }

        public void CheckFinish()
        {
            if (_state.IsEmpty || _state.IsHidden || _state.IsClearHidden || _state.IsNearHidden)
            {
                return;
            }

            if (_state.Waters.Count == _state.MaxCapacity)
            {
                var topColor = GetTopWater().Color;
                bool allSameColor = true;
                bool noIce = true;

                foreach (var water in _state.Waters)
                {
                    if (water.Color != topColor)
                    {
                        allSameColor = false;
                        break;
                    }
                    if (water.Item == WaterItem.Ice)
                    {
                        noIce = false;
                        break;
                    }
                }

                if (allSameColor && noIce)
                {
                    SetFinished(true);
                    EventManager.Trigger(GameEvents.OnLevelComplete);
                }
            }
        }

        public void UpdateVisuals()
        {
            UpdateWaterVisuals();
            UpdateBottleVisuals();
        }

        private void UpdateWaterVisuals()
        {
            for (int i = 0; i < waterImg.Count; i++)
            {
                waterImg[i].gameObject.SetActive(i < _state.Waters.Count);
                if (i < _state.Waters.Count)
                {
                    var water = _state.Waters[i];
                    waterImg[i].SetColorState(GetWaterItemType(water), GetWaterColor(water));
                    waterImg[i].SetHide(water.IsHidden);
                }
            }
        }

        private void UpdateBottleVisuals()
        {
            spineGo.gameObject.SetActive(!_state.IsEmpty);
            if (!_state.IsEmpty)
            {
                var topWater = GetTopWater();
                if (topWater.Color < 1000)
                {
                    spine.AnimationState.SetAnimation(0, GetSpineAnimationName(topWater.Color), false);
                }
            }
        }

        private ItemType GetWaterItemType(Water water)
        {
            if (water.Color >= 1000)
            {
                return (ItemType)water.Color;
            }
            return ItemType.UseColor;
        }

        private Color GetWaterColor(Water water)
        {
            if (water.Color >= 1000)
            {
                return Color.white;
            }
            return LevelManager.Instance.waterColor[water.Color - 1];
        }

        private string GetSpineAnimationName(int color)
        {
            return $"water_{color}";
        }

        public int GetMaxCapacity()
        {
            return _state.MaxCapacity;
        }

        public int GetWaterCount()
        {
            return _state.Waters.Count;
        }

        public void PlayFillAnimation(int amount, int color)
        {
            StartCoroutine(CoroutinePlayFillAnimation(amount, color));
        }

        private System.Collections.IEnumerator CoroutinePlayFillAnimation(int amount, int color)
        {
            float fillDuration = 0.46f;
            yield return new WaitForSeconds(fillDuration);

            int startIdx = _state.TopIndex + 1 - amount;
            if (color < 1000)
            {
                spineGo.gameObject.SetActive(true);
                SetSpinePosition(startIdx);
                spineGo.DOMove(spineNode[_state.TopIndex + 1].position, fillDuration).SetEase(Ease.Linear);
            }
            else
            {
                if (startIdx >= 0)
                {
                    SetSpinePosition(startIdx);
                }
            }

            float fillTime = fillDuration / amount;
            if (color > 1000)
            {
                fillTime = 0.1f;
            }

            for (int i = 0; i < amount; i++)
            {
                waterImg[startIdx + i].waterImg.fillAmount = 0;
            }

            for (int i = 0; i < amount; i++)
            {
                waterImg[startIdx + i].PlayFillAnim(fillTime);
                yield return new WaitForSeconds(fillTime);
            }

            UpdateVisuals();
            CheckFinish();
        }

        private void SetSpinePosition(int node)
        {
            var useNode = node;
            if (useNode - 1 < _state.Waters.Count)
            {
                for (int i = node - 1; i >= 0; i--)
                {
                    if (_state.Waters[i].Color < 1000)
                    {
                        useNode = i + 1;
                        break;
                    }
                }
            }
            spineGo.localPosition = spineNode[useNode].localPosition;
        }

        private void InitializeVisuals()
        {
            finishGo.SetActive(_state.IsFinished);
            freezeGo.gameObject.SetActive(_state.IsFrozen);
            UpdateVisuals();
        }
    }
} 