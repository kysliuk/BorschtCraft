using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotViewManager : IItemSlotViewManager
    {
        private DiContainer _diContainer;
        private SignalBus _signalBus;
        private List<ConsumedViewModelMapping> _viewModelMappings;
        private Dictionary<Type, IManagedConsumedView> _childViews;
        private readonly List<IConsumedViewModel> _activeViewModels = new List<IConsumedViewModel>();
        private string _slotNameForLogging;

        public void Initialize(Transform slotTransform, DiContainer diContainer, SignalBus signalBus, List<ConsumedViewModelMapping> viewModelMappings, Dictionary<Type, IManagedConsumedView> childViews)
        {
            _slotNameForLogging = slotTransform != null ? slotTransform.name : "UnknownSlot";
            _diContainer = diContainer;
            this._signalBus = signalBus;
            _viewModelMappings = viewModelMappings;
            _childViews = childViews;

            if (_childViews != null)
            {
                foreach (var managedView in _childViews.Values)
                {
                    managedView?.DetachViewModel();
                }
            }
        }

        public void DisplayItem(IConsumed itemToDisplay, IConsumed overallRootItem)
        {
            ClearManagedViews();

            if (itemToDisplay == null)
            {
                return;
            }

            if (_diContainer == null || _childViews == null || _viewModelMappings == null)
            {
                Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: Crucial dependencies are null. DIContainer: {_diContainer == null}, ChildViews: {_childViews == null}, ViewModelMappings: {_viewModelMappings == null}");
                return;
            }

            bool topItemIsCookedResult = overallRootItem is ICooked;
            IConsumed currentDisplayLayer = itemToDisplay;
            int layerProcessingCount = 0;

            while (currentDisplayLayer != null && layerProcessingCount < 10)
            {
                layerProcessingCount++;
                Type modelType = currentDisplayLayer.GetType();

                bool shouldDisplayThisLayer = true;
                if (topItemIsCookedResult &&
                    currentDisplayLayer != overallRootItem &&
                    currentDisplayLayer is ICookable)
                {
                    Logger.LogInfo($"Slot {_slotNameForLogging}", $"DisplayItem: Suppressing view for underlying ICookable layer '{modelType.Name}' because top item ('{overallRootItem.GetType().Name}') was ICooked.");
                    shouldDisplayThisLayer = false;
                }

                if (shouldDisplayThisLayer)
                {
                    if (!_childViews.TryGetValue(modelType, out IManagedConsumedView managedView) || managedView == null)
                    {
                        Logger.LogWarning($"Slot {_slotNameForLogging}", $"DisplayItem: No active child view found or view is null for model type: {modelType.Name}.");
                    }
                    else
                    {
                        var mapping = _viewModelMappings.FirstOrDefault(m => m.ConsumedModelType == modelType);
                        if (mapping == null)
                        {
                            Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: No ViewModel mapping found for Consumed type: {modelType.Name}");
                        }
                        else
                        {
                            IConsumedViewModel newViewModelInstance = null;
                            try
                            {
                                object instantiatedObject = _diContainer.Instantiate(mapping.ViewModelType, new object[] { currentDisplayLayer, this._signalBus });
                                newViewModelInstance = instantiatedObject as IConsumedViewModel;

                                if (newViewModelInstance == null)
                                {
                                    Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: Instantiated object of type '{instantiatedObject?.GetType().Name}' for '{mapping.ViewModelType.Name}' could not be cast to IConsumedViewModel.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: EXCEPTION during ViewModel instantiation for {mapping.ViewModelType.Name}. Error: {ex.ToString()}");
                                currentDisplayLayer = null;
                                continue;
                            }

                            if (newViewModelInstance != null)
                            {
                                try
                                {
                                    managedView.AttachViewModel(newViewModelInstance);
                                    newViewModelInstance.SetVisibility(true);
                                    _activeViewModels.Add(newViewModelInstance);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: EXCEPTION during AttachViewModel or SetVisibility for {newViewModelInstance.GetType().Name}. Error: {ex.ToString()}");
                                    currentDisplayLayer = null;
                                    continue;
                                }
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
                    currentDisplayLayer = null;
                }
            }

            if (layerProcessingCount >= 10)
            {
                Logger.LogWarning($"Slot {_slotNameForLogging}", $"DisplayItem: Layer processing loop hit safety limit ({layerProcessingCount}). This might indicate a circular wrapping or too deep an item structure.");
            }
        }

        public void ClearView()
        {
            ClearManagedViews();
        }

        private void ClearManagedViews()
        {
            if (_activeViewModels != null)
            {
                // Optional: Explicitly hide view models before detaching, though DetachViewModel should handle cleanup.
                // foreach (var vm in _activeViewModels)
                // {
                //     vm?.SetVisibility(false);
                // }
            }

            if (_childViews != null)
            {
                foreach (var managedView in _childViews.Values)
                {
                    if (managedView != null)
                    {
                        try
                        {
                            managedView.DetachViewModel();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError($"Slot {_slotNameForLogging}", $"ClearManagedViews: Exception during DetachViewModel: {ex.Message}");
                        }
                    }
                }
            }

            if(_activeViewModels != null)  _activeViewModels.Clear();
        }

        public void OnDestroy()
        {
            ClearView();
        }
    }
}
