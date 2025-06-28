using UnityEngine;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView<T> : MonoBehaviour where T : IConsumed
    {
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
        }

        private void OnEnable()
        {
            _consumedViewModel.SetParentSlotViewModel(GetComponentInParent<SlotView>()?.SlotViewModel);
        }
    }
}
