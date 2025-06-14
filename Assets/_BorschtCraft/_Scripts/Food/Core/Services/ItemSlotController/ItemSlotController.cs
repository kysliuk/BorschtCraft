using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using BorschtCraft.Food.UI.DisplayLogic; // Added for SlotType
using BorschtCraft.Food.UI.Factories;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotController : MonoBehaviour, IItemSlot
    {
        [Header("Slot Configuration")]
        public SlotType SlotContextType = SlotType.Unknown; // Public field to be set in Inspector

        [Inject] private DiContainer _diContainer;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings;

        [InjectOptional] private IItemLayerProcessor _itemLayerProcessor;
        [InjectOptional] private IViewModelFactory _viewModelFactory;

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

            if (_itemLayerProcessor == null)
            {
                _itemLayerProcessor = new ItemLayerProcessor();
            }
            if (_viewModelFactory == null)
            {
                _viewModelFactory = new ViewModelFactory(_diContainer, _signalBus, _viewModelMappings);
            }

            _viewManager = new ItemSlotViewManager();
            // Pass the public SlotContextType field to the Initialize method
            _viewManager.Initialize(transform, _childViews, _itemLayerProcessor, _viewModelFactory, this.SlotContextType);
        }

        public bool TrySetItem(IConsumed newItem)
        {
            this.CurrentItemInSlot = newItem;
            // ItemSlotViewManager.DisplayItem expects itemToDisplay and overallRootItem.
            // As per previous refactoring, overallRootItem is the same as itemToDisplay for the root call.
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