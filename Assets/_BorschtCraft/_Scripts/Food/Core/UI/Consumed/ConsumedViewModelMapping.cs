using System;

namespace BorschtCraft.Food.UI
{
    public class ConsumedViewModelMapping
    {
        public Type ConsumedModelType { get; }
        public Type ViewModelType { get; }

        public ConsumedViewModelMapping(Type consumedModelType, Type viewModelType)
        {
            ConsumedModelType = consumedModelType;
            ViewModelType = viewModelType;
        }
    }
}
