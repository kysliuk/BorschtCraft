using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using BorschtCraft.Food.UI; // For IManagedConsumedView, ConsumedViewModelMapping

namespace BorschtCraft.Food.Core.Interfaces
{
    public interface IItemSlotViewManager
    {
        void Initialize(Transform slotTransform, DiContainer diContainer, SignalBus signalBus, List<ConsumedViewModelMapping> viewModelMappings, Dictionary<Type, IManagedConsumedView> childViews);
        void DisplayItem(IConsumed itemToDisplay, IConsumed overallRootItem);
        void ClearView();
        void OnDestroy(); // For potential cleanup
    }
}
