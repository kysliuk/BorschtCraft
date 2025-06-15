using UnityEngine;
using UniRx;
using System;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView<T> : MonoBehaviour, IConsumedManagedView where T : IConsumed
    {
        public Type ModelType => typeof(T);

        protected IConsumedViewModel _consumedViewModel;
        protected SpriteRenderer _spriteRenderer;
        protected SlotView _parentSlotView;

        [Inject]
        public void Construct(IConsumedViewModel consumedViewModel)
        {
            _consumedViewModel = consumedViewModel;
        }

        protected virtual void EnableVisibility(bool enable)
        {
            _spriteRenderer.enabled = enable;
        }

        #region Unity behaviour
        protected virtual void Awake()
        {
            _consumedViewModel?.IsVisible.Subscribe(EnableVisibility).AddTo(this);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _consumedViewModel.SetParentSlotViewModel(GetComponentInParent<SlotView>()?.SlotViewModel);
        }
        #endregion
    }
}
