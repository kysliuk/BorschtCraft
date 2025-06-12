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
            Logger.LogInfo(this, $"Slot {gameObject.name} TrySetItem: {(newItem == null ? "null" : newItem.GetType().Name)}");

            ClearSlotView();

            CurrentItemInSlot = newItem;

            if (CurrentItemInSlot == null)
                return true;

            IConsumed currentLayer = CurrentItemInSlot;

            while (currentLayer != null)
            {
                Type modelType = currentLayer.GetType();
                Logger.LogInfo(this, $"Slot {gameObject.name} processing layer: {modelType.Name}");

                if (_childViews.TryGetValue(modelType, out var managedView))
                {
                    var mapping = _viewModelMappings.FirstOrDefault(m => m.ConsumedModelType == modelType);

                    if (mapping != null)
                    {
                        var viewModelArgs = new object[] { currentLayer, _signalBus };
                        var newViewmodel = _diContainer.Instantiate(mapping.ViewModelType, viewModelArgs) as IConsumedViewModel;
                        managedView.AttachViewModel(newViewmodel);
                        newViewmodel.SetVisibility(true);
                        _activeViewModels.Add(newViewmodel);
                        Logger.LogInfo(this, $"Slot {gameObject.name} attached view model {newViewmodel.GetType().Name} for model type {modelType.Name} on {managedView.GetGameObject().name}.");
                    }
                    else
                        Logger.LogError(this, $"Slot {gameObject.name}: No ViewModel mapping found for Consumed type: {modelType.Name}");
                }
                else
                    Logger.LogWarning(this, $"Slot {gameObject.name}: No child view found for Consumed layer type: {modelType.Name}");

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