using Zenject;
using BorschtCraft.Food.Signals;

namespace BorschtCraft.Food
{
    public class ConsumableInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IItemConsumptionService>().To<ItemConsumptionService>()
                .AsSingle()
                .NonLazy();

            Container.DeclareSignal<ConsumableInteractionRequestSignal<BreadStack, BreadRaw>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<SaloStack, Salo>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<GarlicStack, Garlic>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<OnionStack, Onion>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<MustardStack, Mustard>>();
            Container.DeclareSignal<ConsumableInteractionRequestSignal<HorseradishStack, Horseradish>>();

            Container.DeclareSignal<ConsumedItemCreatedSignal<BreadStack, BreadRaw>>();
            Container.DeclareSignal<ConsumedItemCreatedSignal<SaloStack, Salo>>();
            Container.DeclareSignal<ConsumedItemCreatedSignal<GarlicStack, Garlic>>();
            Container.DeclareSignal<ConsumedItemCreatedSignal<OnionStack, Onion>>();
            Container.DeclareSignal<ConsumedItemCreatedSignal<MustardStack, Mustard>>();
            Container.DeclareSignal<ConsumedItemCreatedSignal<HorseradishStack, Horseradish>>();

        }
    }
}
