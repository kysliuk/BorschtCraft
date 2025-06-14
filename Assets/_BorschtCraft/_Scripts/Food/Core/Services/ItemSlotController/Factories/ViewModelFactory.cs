using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food.UI.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _diContainer;
        private readonly SignalBus _signalBus;
        private readonly List<ConsumedViewModelMapping> _viewModelMappings;

        public ViewModelFactory(DiContainer diContainer, SignalBus signalBus, List<ConsumedViewModelMapping> viewModelMappings)
        {
            _diContainer = diContainer;
            _signalBus = signalBus;
            _viewModelMappings = viewModelMappings;
        }

        public IConsumedViewModel CreateViewModel(IConsumed itemLayer)
        {
            if (itemLayer == null) return null;

            Type modelType = itemLayer.GetType();
            var mapping = _viewModelMappings.FirstOrDefault(m => m.ConsumedModelType == modelType);

            if (mapping == null)
            {
                Logger.LogError(this, $"CreateViewModel: No ViewModel mapping found for Consumed type: {modelType.Name}");
                return null;
            }

            try
            {
                object instantiatedObject = _diContainer.Instantiate(mapping.ViewModelType, new object[] { itemLayer, _signalBus });
                IConsumedViewModel newViewModelInstance = instantiatedObject as IConsumedViewModel;

                if (newViewModelInstance == null)
                {
                    Logger.LogError(this, $"CreateViewModel: Instantiated object of type '{instantiatedObject?.GetType().Name}' for '{mapping.ViewModelType.Name}' could not be cast to IConsumedViewModel.");
                }
                return newViewModelInstance;
            }
            catch (Exception ex)
            {
                Logger.LogError(this, $"CreateViewModel: EXCEPTION during ViewModel instantiation for {mapping.ViewModelType.Name}. Item: {modelType.Name}. Error: {ex.ToString()}");
                return null;
            }
        }
    }
}
