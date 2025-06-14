using Zenject;
using BorschtCraft.Food.Core.Services.ConsumingService.Strategies; // Added for strategies
using BorschtCraft.Food; // Added for IConsumingService, ICombiningService etc.

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

        }
    }
}
