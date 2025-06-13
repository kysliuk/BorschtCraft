using BorschtCraft.Food.Signals;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ConsumedView<T1, T2> : MonoBehaviour, IManagedConsumedView, IPointerClickHandler
        where T1 : IConsumedViewModel
        where T2 : Consumed
    {
        protected T1 _viewModel;
        protected SpriteRenderer _spriteRenderer;

        private SignalBus _signalBus;
        private IItemSlot _parentSlotModel;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
                Logger.LogError(this, $"{gameObject.name} requires a SpriteRenderer component.");
        }

        [Inject]
        public void Construct(SignalBus signalBus, IItemSlot parentSlotModel)
        {
            _signalBus = signalBus;
            _parentSlotModel = parentSlotModel;
        }

        public Type GetConsumedModelType()
        {
            return typeof(T2);
        }

        public void AttachViewModel(IConsumedViewModel viewModel)
        {
            if (viewModel is T1 typedViewModel)
                InitializeWithViewModel(typedViewModel);
            else if (viewModel != null)
                Logger.LogError(this, $"View model type {viewModel.GetType().Name} does not match expected type {typeof(T1).Name}");
            else
                InitializeWithViewModel(default);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void DetachViewModel()
        {
            InitializeWithViewModel(default);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (_parentSlotModel == null)
            {
                Logger.LogWarning(this, "OnPointerClick: Cannot fire SlotClickedSignal because parent IItemSlot model is null.");
                return;
            }

            Logger.LogInfo(this, $"View clicked. Firing SlotClickedSignal for slot of type {_parentSlotModel.Type}.");
            _signalBus.Fire(new SlotClickedSignal(_parentSlotModel));
        }

        protected void EnableVisibility(bool enable)
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = enable;
            }
        }

        public virtual void InitializeWithViewModel(T1 viewModel)
        {
            _viewModel = viewModel;

            if (_viewModel == null)
            {
                EnableVisibility(false);
                return;
            }

            _viewModel.IsVisible.Subscribe(EnableVisibility).AddTo(this);
        }
    }
}