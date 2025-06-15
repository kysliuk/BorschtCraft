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
            Container.DeclareSignal<ConsumableInteractionRequestSignal>();
            Container.DeclareSignal<ReleaseSlotItemSignal>();

            //Install Consumables
            InstallConsumables();
        }

        private void InstallConsumables()
        {
            new GenericConsumablePrefabInstaller<BreadStack, BreadRaw>().Install(Container, _initialPrice);
            new GenericConsumablePrefabInstaller<SaloStack, Salo>().Install(Container, _initialPrice);
            new GenericConsumablePrefabInstaller<GarlicStack, Garlic>().Install(Container, _initialPrice);
            new GenericConsumablePrefabInstaller<HorseradishStack, Horseradish>().Install(Container, _initialPrice);
            new GenericConsumablePrefabInstaller<MustardStack, Mustard>().Install(Container, _initialPrice);
            new GenericConsumablePrefabInstaller<OnionStack, Onion>().Install(Container, _initialPrice);
        }
    }
}
