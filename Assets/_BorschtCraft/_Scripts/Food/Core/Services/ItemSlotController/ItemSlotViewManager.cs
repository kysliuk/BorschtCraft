using UnityEngine;
using System;
using System.Collections.Generic;
using BorschtCraft.Food.UI.DisplayLogic; // For SlotType and IItemLayerProcessor
using BorschtCraft.Food.UI.Factories;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotViewManager : IItemSlotViewManager
    {
        private IItemLayerProcessor _itemLayerProcessor;
        private IViewModelFactory _viewModelFactory;
        private Dictionary<Type, IManagedConsumedView> _childViews;
        private readonly List<IConsumedViewModel> _activeViewModels = new List<IConsumedViewModel>();
        private string _slotNameForLogging;
        private SlotType _currentSlotType; // Added field to store slot context

        // Updated Initialize method to accept SlotType
        public void Initialize(Transform slotTransform,
                               Dictionary<Type, IManagedConsumedView> childViews,
                               IItemLayerProcessor itemLayerProcessor,
                               IViewModelFactory viewModelFactory,
                               SlotType slotType) // Added slotType parameter
        {
            _slotNameForLogging = slotTransform != null ? slotTransform.name : "UnknownSlot";
            _childViews = childViews;
            _itemLayerProcessor = itemLayerProcessor;
            _viewModelFactory = viewModelFactory;
            _currentSlotType = slotType; // Store the slot type

            if (_childViews != null)
            {
                foreach (var managedView in _childViews.Values)
                {
                    managedView?.DetachViewModel();
                }
            }
        }

        public void DisplayItem(IConsumed itemToDisplay, IConsumed overallRootItem) // overallRootItem is effectively itemToDisplay
        {
            ClearManagedViews();

            if (itemToDisplay == null || _itemLayerProcessor == null || _viewModelFactory == null || _childViews == null)
            {
                Logger.LogError($"Slot {_slotNameForLogging} (Type: {_currentSlotType})", $"DisplayItem: Crucial dependencies are null or itemToDisplay is null. Item: {itemToDisplay == null}, Processor: {_itemLayerProcessor == null}, Factory: {_viewModelFactory == null}, ChildViews: {_childViews == null}");
                return;
            }

            // Pass the stored _currentSlotType to GetLayersToDisplay
            List<IConsumed> layersToDisplay = _itemLayerProcessor.GetLayersToDisplay(itemToDisplay, _currentSlotType);

            foreach (var layer in layersToDisplay)
            {
                Type modelType = layer.GetType();
                if (!_childViews.TryGetValue(modelType, out IManagedConsumedView managedView) || managedView == null)
                {
                    // Updated log to include context and specific item type that has no view
                    Logger.LogWarning($"Slot {_slotNameForLogging} (Type: {_currentSlotType})", $"DisplayItem: No active child view found or view is null for model type: {modelType.FullName}.");
                    continue;
                }

                IConsumedViewModel viewModel = _viewModelFactory.CreateViewModel(layer);
                if (viewModel != null)
                {
                    try
                    {
                        managedView.AttachViewModel(viewModel);
                        viewModel.SetVisibility(true);
                        _activeViewModels.Add(viewModel);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Slot {_slotNameForLogging} (Type: {_currentSlotType})", $"DisplayItem: EXCEPTION during AttachViewModel or SetVisibility for {viewModel.GetType().Name} on view for {modelType.Name}. Error: {ex.ToString()}");
                    }
                }
            }
        }

        public void ClearView()
        {
            ClearManagedViews();
        }

        private void ClearManagedViews()
        {
            if (_childViews != null)
            {
                foreach (var managedView in _childViews.Values)
                {
                    managedView?.DetachViewModel();
                }
            }
            _activeViewModels.Clear();
        }

        public void OnDestroy()
        {
            ClearView();
        }
    }
}
