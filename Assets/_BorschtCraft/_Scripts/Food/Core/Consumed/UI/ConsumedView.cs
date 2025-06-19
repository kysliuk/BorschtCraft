using UnityEngine;
using UniRx;
using System;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView<T> : MonoBehaviour where T : IConsumed
    {
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

            _consumedViewModel?.IsVisible?.Subscribe(SetVisibility).AddTo(this);

            Logger.LogInfo(this, $"Constructed with view model: {_consumedViewModel?.GetType()?.Name}<{typeof(T).Name}>");
        }

        protected virtual void SetVisibility(bool enable)
        {
            Logger.LogInfo(this, $"Setting visibility to {enable} for slot of type {typeof(T).Name}.");
            _spriteRenderer.enabled = enable;
        }
    }
}
