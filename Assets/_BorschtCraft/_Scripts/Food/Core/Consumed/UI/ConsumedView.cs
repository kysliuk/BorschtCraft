using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView<T> : MonoBehaviour where T : IConsumed
    {
        [SerializeField] protected float _scaleFactor = 1.25f;
        [SerializeField] protected float _animationDuration = 0.25f;

        public ConsumedViewModel<T> ConsumedViewModel => _consumedViewModel;

        protected ConsumedViewModel<T> _consumedViewModel;
        protected SpriteRenderer _spriteRenderer;
        protected SlotView _parentSlotView;

        [Inject]
        public void Construct(ConsumedViewModel<T> consumedViewModel)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _consumedViewModel = consumedViewModel;
            _consumedViewModel?.IsVisible?.Subscribe(SetVisibility).AddTo(this);
        }

        protected virtual void SetVisibility(bool enable)
        {
            _spriteRenderer.enabled = enable;

            if (enable)
            {
                AnimateScale();
            }
        }

        protected virtual void AnimateScale()
        {
            var originalScale = transform.localScale;
            var targetScale = originalScale * _scaleFactor;

            transform.DOScale(targetScale, _animationDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, _animationDuration).SetEase(Ease.InQuad);
                });
        }

        private void OnEnable()
        {
            _consumedViewModel.SetParentSlotViewModel(GetComponentInParent<SlotView>()?.SlotViewModel);
        }
    }
}
