using Zenject;
using BorschtCraft.Food.Core.Services.ConsumingService.Strategies;
using BorschtCraft.Food;
using BorschtCraft.Food.UI.DisplayLogic; // Added for ItemLayerProcessor
using BorschtCraft.Food.UI.Factories;    // Added for ViewModelFactory
using BorschtCraft.Food.UI;             // Added for ConsumedViewModelMapping (if not already covered)

namespace BorschtCraft
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Bind Strategies (order of these bindings will determine injection order into List)
            Container.Bind<IConsumptionStrategy>().To<DecorationStrategy>().AsTransient();
            Container.Bind<IConsumptionStrategy>().To<InitialProductionStrategy>().AsTransient();

            // Assuming ConsumingService and CombiningService are already bound elsewhere
            // If not, they would need bindings like:
            // Container.Bind<IConsumingService>().To<ConsumingService>().AsSingle();
            // Container.Bind<ICombiningService>().To<CombiningService>().AsSingle();
            // Container.Bind<IItemSlot[]>().WithId("CookingSlots").FromInstance(new IItemSlot[/*size*/]).AsSingle(); // Example
            // Container.Bind<IItemSlot[]>().WithId("ReleasingSlots").FromInstance(new IItemSlot[/*size*/]).AsSingle(); // Example

            // Bind new services for ItemSlotViewManager
            Container.Bind<IItemLayerProcessor>().To<ItemLayerProcessor>().AsTransient(); // Or AsSingle if stateless and preferred

            // ViewModelFactory depends on DiContainer, SignalBus, List<ConsumedViewModelMapping>
            // These should already be available in the container for injection into ViewModelFactory if bound AsSingle/Transient.
            // Assuming List<ConsumedViewModelMapping> is bound elsewhere (e.g. by a scriptable object installer)
            Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsSingle();

            // ItemSlotController itself is likely a MonoBehaviour attached to a GameObject,
            // so its dependencies (_diContainer, _signalBus, _viewModelMappings, and now optionally
            // _itemLayerProcessor, _viewModelFactory) are typically field-injected by Zenject.
            // No explicit binding change needed for ItemSlotController if it's on a prefab that gets processed.
        }
    }
}
