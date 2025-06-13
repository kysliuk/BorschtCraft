using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotController : MonoBehaviour, IItemSlot
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings;

        public IConsumed CurrentItemInSlot { get; private set; }

        private readonly Dictionary<Type, IManagedConsumedView> _childViews = new Dictionary<Type, IManagedConsumedView>();
        private IItemSlotViewManager _viewManager;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                var managedView = child.GetComponent<IManagedConsumedView>();
                if (managedView != null)
                {
                    Type modelType = managedView.GetConsumedModelType();
                    if (!_childViews.ContainsKey(modelType))
                    {
                        _childViews.Add(modelType, managedView);
                    }
                }
            }

            _viewManager = new ItemSlotViewManager();
            _viewManager.Initialize(transform, _diContainer, _signalBus, _viewModelMappings, _childViews);
        }

        public bool TrySetItem(IConsumed newItem)
        {
            this.CurrentItemInSlot = newItem;
            _viewManager.DisplayItem(this.CurrentItemInSlot, this.CurrentItemInSlot);

            return true;
        }

        public IConsumed ReleaseItem()
        {
            var releasedItem = CurrentItemInSlot;
            CurrentItemInSlot = null;
            ClearSlotView();
            return releasedItem;
        }

        private void ClearSlotView()
        {
            _viewManager?.ClearView();
        }

        private void OnDestroy()
        {
            _viewManager?.OnDestroy();
        }

        public IConsumed GetCurrentItem()
        {
            return this.CurrentItemInSlot;
        }

        public bool IsEmpty()
        {
            return this.CurrentItemInSlot == null;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }
    }
}