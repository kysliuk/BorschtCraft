using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using BorschtCraft.Food.UI;
using BorschtCraft.Food.UI.DisplayLogic;
using BorschtCraft.Food.UI.Factories;

namespace BorschtCraft.Food
{
    public interface IItemSlotViewManager
    {
        public void Initialize(Transform slotTransform, Dictionary<Type, IManagedConsumedView> childViews, IItemLayerProcessor itemLayerProcessor, IViewModelFactory viewModelFactory);
        void DisplayItem(IConsumed itemToDisplay, IConsumed overallRootItem);
        void ClearView();
        void OnDestroy();
    }
}
