using System.Collections.Generic;
// using System.Linq; // No longer needed here
using UnityEngine;
using Zenject;
using System;
using BorschtCraft.Food;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotController : MonoBehaviour, IItemSlot
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings;

        public IConsumed CurrentItemInSlot { get; private set; }

        private readonly Dictionary<Type, IManagedConsumedView> _childViews = new Dictionary<Type, IManagedConsumedView>();
        // private readonly List<IConsumedViewModel> _activeViewModels = new List<IConsumedViewModel>(); // Removed
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
                        // managedView.DetachViewModel(); // This will be handled by ItemSlotViewManager.Initialize
                    }
                }
            }

            _viewManager = new ItemSlotViewManager();
            _viewManager.Initialize(transform, _diContainer, _signalBus, _viewModelMappings, _childViews);
        }

        public bool TrySetItem(IConsumed newItem)
        {
            // Basic validation for dependencies can remain if desired, or be fully delegated.
            // For now, keeping it simple and assuming _viewManager is initialized.
            // if (_viewManager == null)
            // {
            //    Logger.LogError(this, $"Slot {gameObject.name} TrySetItem: _viewManager is null. Awake might not have run or initialization failed.");
            //    return false;
            // }

            this.CurrentItemInSlot = newItem;
            _viewManager.DisplayItem(this.CurrentItemInSlot, this.CurrentItemInSlot); // Passing CurrentItemInSlot as overallRootItem

            return true; // Assuming success unless an exception is thrown by DisplayItem
        }

        public IConsumed ReleaseItem()
        {
            var releasedItem = CurrentItemInSlot;
            CurrentItemInSlot = null;
            _viewManager.ClearView(); // Use view manager to clear
            return releasedItem;
        }

        private void ClearSlotView() // This method now delegates
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