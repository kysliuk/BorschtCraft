using System;
using System.Collections.Generic;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotViewModel
    {
        private SignalBus _signalBus;
        private List<ConsumedViewModelMapping> _viewModelMappings;
        private readonly Dictionary<Type, IManagedConsumedView> _childViews = new Dictionary<Type, IManagedConsumedView>();


        public void AddChildItemView(Type type, IManagedConsumedView view)
        {
            _childViews.Add(type, view);
        }
    }
}
