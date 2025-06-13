using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UniRx;
using BorschtCraft.Food.Signals;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotView : MonoBehaviour
    {
        [Inject] private IItemSlot _slotModel;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings;
        [Inject] private DiContainer _diContainer;

        private readonly Dictionary<Type, IManagedConsumedView> _childViews = new Dictionary<Type, IManagedConsumedView>();
        private IItemSlotViewManager _viewManager;

        private void Awake()
        {
            foreach (var managedView in GetComponentsInChildren<IManagedConsumedView>(true))
            {
                Type modelType = managedView.GetConsumedModelType();
                if (!_childViews.ContainsKey(modelType))
                {
                    _childViews.Add(modelType, managedView);
                }
            }
        }

        [Inject]
        public void Initialize()
        {
            _viewManager = new ItemSlotViewManager();
            _viewManager.Initialize(transform, _diContainer, _signalBus, _viewModelMappings, _childViews);

            _slotModel.CurrentItem
                .Subscribe(item => _viewManager.DisplayItem(item, item))
                .AddTo(this);
        }

        public void OnPointerClick()
        {
            _signalBus.Fire(new SlotClickedSignal(_slotModel));
        }

        private void OnDestroy()
        {
            _viewManager?.OnDestroy();
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }
    }
}