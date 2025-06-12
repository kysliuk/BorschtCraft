using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using System;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<ConsumedViewModelMapping> _viewModelMappings;

        public IConsumed CurrentItemInSlot { get; private set; }

        private Dictionary<Type, IManagedConsumedView> _childViews = new Dictionary<Type, IManagedConsumedView>();
        private List<IConsumedViewModel> _activeViewModels = new List<IConsumedViewModel>();

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
                        managedView.DetachViewModel();
                        Logger.LogInfo(this, $"Slot {gameObject.name} cached child view for {modelType.Name} on {managedView.GetGameObject().name}");
                    }
                    else
                    {
                        Logger.LogWarning(this, $"Slot {gameObject.name} has multiple child views for model type {modelType.Name}. Using first one found: {managedView.GetGameObject().name}.");
                    }
                }
            }
        }

        public bool TrySetItem(IConsumed newItem)
        {
            if (_childViews == null)
                return false;

            ClearSlotView();
            CurrentItemInSlot = newItem;

            if (CurrentItemInSlot == null)
                return true;

            IConsumed currentDisplayLayer = CurrentItemInSlot;
            var layerProcessingCount = 0;

            while (currentDisplayLayer != null && layerProcessingCount < 10)
            {
                layerProcessingCount++;
                Type modelType = currentDisplayLayer.GetType();
                Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: Processing layer {layerProcessingCount}: {modelType.Name}");

                if (!_childViews.TryGetValue(modelType, out IManagedConsumedView managedView) || managedView == null)
                {
                    Logger.LogWarning(this, $"Slot {gameObject.name} TrySetItem: No active child view found or view is null for model type: {modelType.Name}.");

                }
                else 
                {
                    var mapping = _viewModelMappings.FirstOrDefault(m => m.ConsumedModelType == modelType);
                    if (mapping == null)
                    {
                        Logger.LogError(this, $"Slot {gameObject.name} TrySetItem: No ViewModel mapping found for Consumed type: {modelType.Name}");
                    }
                    else
                    {
                        Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: Found mapping for {modelType.Name} to ViewModel {mapping.ViewModelType.Name}. Instantiating...");
                        var viewModelArgs = new object[] { currentDisplayLayer, _signalBus };
                        IConsumedViewModel newViewModelInstance = null;
                        try
                        {
                            object instantiatedObject = _diContainer.Instantiate(mapping.ViewModelType, viewModelArgs);
                            newViewModelInstance = instantiatedObject as IConsumedViewModel;

                            if (newViewModelInstance == null)
                            {
                                Logger.LogError(this, $"Slot {gameObject.name} TrySetItem: Instantiated object of type '{instantiatedObject?.GetType().Name}' for '{mapping.ViewModelType.Name}' could not be cast to IConsumedViewModel.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(this, $"Slot {gameObject.name} TrySetItem: EXCEPTION during ViewModel instantiation for {mapping.ViewModelType.Name}. Args: {currentDisplayLayer.GetType().Name}, SignalBus. Error: {ex.ToString()}");
                            currentDisplayLayer = null;
                            continue;
                        }

                        if (newViewModelInstance != null)
                        {
                            Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: ViewModel {newViewModelInstance.GetType().Name} instantiated. Attaching to view {managedView.GetGameObject().name}.");
                            try
                            {
                                managedView.AttachViewModel(newViewModelInstance);
                                newViewModelInstance.SetVisibility(true);
                                _activeViewModels.Add(newViewModelInstance);
                                Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: ViewModel attached and configured for {modelType.Name}.");
                            }
                            catch (System.Exception ex)
                            {
                                Logger.LogError(this, $"Slot {gameObject.name} TrySetItem: EXCEPTION during AttachViewModel or SetVisibility for {newViewModelInstance.GetType().Name} on {managedView.GetGameObject().name}. Error: {ex.ToString()}");
                                currentDisplayLayer = null;
                                continue;
                            }
                        }
                    }
                }
                if (currentDisplayLayer is Consumed consumedLayerInstance)
                {
                    currentDisplayLayer = consumedLayerInstance.WrappedItem;
                }
                else
                {
                    Logger.LogWarning(this, $"Slot {gameObject.name} TrySetItem: currentDisplayLayer ({currentDisplayLayer?.GetType().Name}) is not a 'Consumed' instance, cannot get DirectlyConsumedItem. Ending layer processing.");
                    currentDisplayLayer = null;
                }
                Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: Next layer to process: {(currentDisplayLayer == null ? "null" : currentDisplayLayer.GetType().Name)}");
            }
            return true;
        }
        public IConsumed ReleaseItem()
        {
            var releasedItem = CurrentItemInSlot;
            Logger.LogInfo(this, $"Slot {gameObject.name} Releasing item: {(releasedItem == null ? "null" : releasedItem.GetType().Name)}");
            ClearSlotView();

            CurrentItemInSlot = null;

            return releasedItem;
        }

        private void ClearSlotView()
        {
            foreach(var managedView in _childViews.Values)
            {
                managedView.DetachViewModel();
            }

            _activeViewModels.Clear();
            Logger.LogInfo(this, $"Slot {gameObject.name} cleared. All child views detached.");
        }

        private void OnDestroy()
        {
            ClearSlotView();
        }


    }
}