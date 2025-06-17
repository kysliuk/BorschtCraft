using UnityEngine;
using UniRx;
using System;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView<T> : MonoBehaviour where T : IConsumed
    {
        public Type ModelType => typeof(T);

        protected ConsumedViewModel<T> _consumedViewModel;
        protected SpriteRenderer _spriteRenderer;
        protected SlotView _parentSlotView;

        [Inject]
        public void Construct(ConsumedViewModel<T> consumedViewModel)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
                Logger.LogWarning(this, $"{nameof(SpriteRenderer)} is null in {nameof(ConsumedView<T>)}");

            _consumedViewModel = consumedViewModel;

            _consumedViewModel.SetParentSlotViewModel(GetComponentInParent<SlotView>()?.SlotViewModel);

            _consumedViewModel?.IsVisible?.Subscribe(enabled => _spriteRenderer.enabled = enabled).AddTo(this);

            Logger.LogInfo(this, $"Constructed with view model: {_consumedViewModel?.GetType()?.Name}<{typeof(T).Name}>");
        }
    }
}
