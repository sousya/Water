using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WaterGame.Events;

namespace WaterGame.Services
{
    public class AnimationService : IService
    {
        private readonly Dictionary<string, Tween> _activeTweens = new Dictionary<string, Tween>();
        private readonly Dictionary<string, Sequence> _activeSequences = new Dictionary<string, Sequence>();

        public void Initialize()
        {
            DOTween.SetTweensCapacity(200, 125);
        }

        public void Cleanup()
        {
            StopAllAnimations();
        }

        public void PlayWaterMoveAnimation(Transform source, Transform target, float duration)
        {
            var water = source.DOMove(target.position, duration)
                .SetEase(Ease.Linear);

            water.OnComplete(() => {
                // 水移动完成后的处理
            });
        }

        public void PlayBottleRotateAnimation(Transform bottle, Vector3 targetRotation, float duration, System.Action onComplete = null)
        {
            bottle.DORotate(targetRotation, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void PlayFillAnimation(Transform water, float duration)
        {
            var scale = water.localScale;
            water.localScale = new Vector3(scale.x, 0, scale.z);
            water.DOScaleY(scale.y, duration)
                .SetEase(Ease.OutQuad);
        }

        public void PlayEmptyAnimation(Transform water, float duration)
        {
            var scale = water.localScale;
            water.DOScaleY(0, duration)
                .SetEase(Ease.InQuad);
        }

        public void PlayItemUseAnimation(Transform item, Transform target, float duration)
        {
            item.DOMove(target.position, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    // 道具使用完成后的处理
                });
        }

        public void PlayFinishAnimation(Transform bottle, float duration)
        {
            bottle.DOScale(Vector3.one * 1.2f, duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    bottle.DOScale(Vector3.one, duration * 0.5f)
                        .SetEase(Ease.InQuad);
                });
        }

        public void PlayClearAnimation(Transform bottle, float duration)
        {
            bottle.DOScale(Vector3.zero, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => {
                    bottle.gameObject.SetActive(false);
                });
        }

        public void StopAllAnimations()
        {
            foreach (var tween in _activeTweens.Values)
            {
                tween.Kill();
            }
            foreach (var sequence in _activeSequences.Values)
            {
                sequence.Kill();
            }
            _activeTweens.Clear();
            _activeSequences.Clear();
        }
    }
} 