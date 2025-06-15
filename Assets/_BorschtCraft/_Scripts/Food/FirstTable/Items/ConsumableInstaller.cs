using Zenject;
using BorschtCraft.Food.Signals;
using UnityEngine;
using BorschtCraft.Food.UI;

namespace BorschtCraft.Food
{
    public class ConsumableInstaller : MonoInstaller
    {
        [SerializeField] private int _initialPrice = 10; //To be changed with gameconfig
        public override void InstallBindings()
        {
            Container.Bind<MonoBehaviour>().WithId("CoroutineHost").FromInstance(this).AsSingle();

            //Bind Signals
            InstallSignals();

            //Install Consumables
            InstallConsumables();

            //Install Consumed
            InstallConsumed();
        }

        private void InstallConsumables()
        {
            new GenericConsumableInstaller<BreadStack, BreadRaw>().Install(Container, _initialPrice);
            new GenericConsumableInstaller<SaloStack, Salo>().Install(Container, _initialPrice);
            new GenericConsumableInstaller<GarlicStack, Garlic>().Install(Container, _initialPrice);
            new GenericConsumableInstaller<HorseradishStack, Horseradish>().Install(Container, _initialPrice);
            new GenericConsumableInstaller<MustardStack, Mustard>().Install(Container, _initialPrice);
            new GenericConsumableInstaller<OnionStack, Onion>().Install(Container, _initialPrice);
        }

        private void InstallConsumed()
        {
            new GenericConsumedInstaller<BreadRaw>().Install(Container);
            new GenericConsumedInstaller<BreadCooked>().Install(Container);
            new GenericConsumedInstaller<Salo>().Install(Container);
            new GenericConsumedInstaller<Garlic>().Install(Container);
            new GenericConsumedInstaller<Horseradish>().Install(Container);
            new GenericConsumedInstaller<Mustard>().Install(Container);
            new GenericConsumedInstaller<Onion>().Install(Container);
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<ConsumableInteractionRequestSignal>();
            Container.DeclareSignal<ReleaseSlotItemSignal>();

            GenericInstallSignal<BreadRaw>();
            GenericInstallSignal<BreadCooked>();
            GenericInstallSignal<Salo>();
            GenericInstallSignal<Garlic>();
            GenericInstallSignal<Horseradish>();
            GenericInstallSignal<Mustard>();
            GenericInstallSignal<Onion>();
            GenericInstallSignal<Garlic>();
        }

        private void GenericInstallSignal<T>() where T : IConsumed
        {
            Container.DeclareSignal<SlotItemChangedSignal<T>>();
        }
    }
}
