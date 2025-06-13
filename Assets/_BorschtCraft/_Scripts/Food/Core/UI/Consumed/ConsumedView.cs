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
        protected SignalBus _signalBus;
        protected ItemSlotController _parentSlotController;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if(_spriteRenderer == null)
                Logger.LogError(this, $"{gameObject.name} requires a SpriteRenderer component. Please add one to the GameObject.");

            _parentSlotController = GetComponentInParent<ItemSlotController>();
            if(_parentSlotController == null)
                Logger.LogError(this, $"{this.GetType().Name} must be a child of ItemSlotController. Please check your hierarchy.");
        }

        public Type GetConsumedModelType()
        {
            return typeof(T2);
        }

        public void AttachViewModel(IConsumedViewModel viewModel)
        {
            if (viewModel is T1 typedViewModel)
                InitializeWithViewModel(typedViewModel);
            else if(viewModel != null)
                throw new ArgumentException($"View model type {viewModel.GetType().Name} does not match expected type {typeof(T1).Name} for view {this.GetType().Name} and model {typeof(T2).Name}");
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
            Logger.LogInfo(this, $"View model exists: {_viewModel != null}. {_viewModel?.GetType()?.Name}");
            if (_parentSlotController != null)
                _signalBus.Fire(new SlotClickedSignal(_parentSlotController));
        }

        protected void EnableVisibility(bool enable)
        {
            _spriteRenderer.enabled = enable;
        }

        public virtual void InitializeWithViewModel(T1 viewModel)
        {
            _viewModel = viewModel;
            Logger.LogInfo(this, $"Constructed with view model: {_viewModel?.GetType()?.Name}");

            if (_viewModel == null)
            {
                EnableVisibility(false);
                return;
            }

            _viewModel.IsVisible.Subscribe(EnableVisibility).AddTo(this);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}