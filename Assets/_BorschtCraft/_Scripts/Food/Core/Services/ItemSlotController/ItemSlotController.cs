using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using BorschtCraft.Food.UI.DisplayLogic; // Added
using BorschtCraft.Food.UI.Factories;    // Added

namespace BorschtCraft.Food.UI
{
    public class ItemSlotController : MonoBehaviour, IItemSlot
    {
        [Inject] private DiContainer _diContainer; // Still needed for ViewModelFactory
        [Inject] private SignalBus _signalBus;       // Still needed for ViewModelFactory
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings; // Still needed for ViewModelFactory

        // Optional injections for the new services, if they are bound in Zenject
        // Otherwise, ItemSlotController can new them up if they have no Zenject dependencies themselves.
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

            // Instantiate services if not injected by Zenject
            if (_itemLayerProcessor == null)
            {
                _itemLayerProcessor = new ItemLayerProcessor();
            }
            if (_viewModelFactory == null)
            {
                // ViewModelFactory needs DiContainer, SignalBus, Mappings
                _viewModelFactory = new ViewModelFactory(_diContainer, _signalBus, _viewModelMappings);
            }

            _viewManager = new ItemSlotViewManager();
            // Updated Initialize call for _viewManager
            _viewManager.Initialize(transform, _childViews, _itemLayerProcessor, _viewModelFactory);
        }

        public bool TrySetItem(IConsumed newItem)
        {
            this.CurrentItemInSlot = newItem;
            _viewManager.DisplayItem(this.CurrentItemInSlot, this.CurrentItemInSlot); // Second param was overallRootItem, now just pass current
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