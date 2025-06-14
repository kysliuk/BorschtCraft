using UnityEngine;
using System;
using System.Collections.Generic;
// Removed: using Zenject;
// Removed: using System.Linq;
using BorschtCraft.Food.UI.DisplayLogic;
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

        // Updated Initialize method
        public void Initialize(Transform slotTransform,
                               Dictionary<Type, IManagedConsumedView> childViews,
                               IItemLayerProcessor itemLayerProcessor,
                               IViewModelFactory viewModelFactory)
        {
            _slotNameForLogging = slotTransform != null ? slotTransform.name : "UnknownSlot";
            _childViews = childViews;
            _itemLayerProcessor = itemLayerProcessor;
            _viewModelFactory = viewModelFactory;

            if (_childViews != null)
            {
                foreach (var managedView in _childViews.Values)
                {
                    managedView?.DetachViewModel();
                }
            }
        }

        public void DisplayItem(IConsumed itemToDisplay, IConsumed overallRootItem) // overallRootItem is now effectively itemToDisplay
        {
            ClearManagedViews();

            if (itemToDisplay == null || _itemLayerProcessor == null || _viewModelFactory == null || _childViews == null)
            {
                Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: Crucial dependencies are null or itemToDisplay is null. Item: {itemToDisplay == null}, Processor: {_itemLayerProcessor == null}, Factory: {_viewModelFactory == null}, ChildViews: {_childViews == null}");
                return;
            }

            List<IConsumed> layersToDisplay = _itemLayerProcessor.GetLayersToDisplay(itemToDisplay);

            foreach (var layer in layersToDisplay)
            {
                Type modelType = layer.GetType();
                if (!_childViews.TryGetValue(modelType, out IManagedConsumedView managedView) || managedView == null)
                {
                    Logger.LogWarning($"Slot {_slotNameForLogging}", $"DisplayItem: No active child view found or view is null for model type: {modelType.Name}.");
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
                        Logger.LogError($"Slot {_slotNameForLogging}", $"DisplayItem: EXCEPTION during AttachViewModel or SetVisibility for {viewModel.GetType().Name} on view for {modelType.Name}. Error: {ex.ToString()}");
                    }
                }
                // If CreateViewModel returns null, it has already logged the error.
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
                    managedView?.DetachViewModel(); // Exception handling for Detach can be inside IManagedConsumedView if preferred
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
