using Zenject;
using UnityEngine;

namespace BorschtCraft.Food.FirstTable
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private int _initialPrice = 10; //To be changed with levelconfig
        public override void InstallBindings()
        {
            //Bind slot registry
            Container.Bind<ISlotRegistry>().To<SlotRegistry>().AsSingle();

            //Install Slot Signals
            new SlotSignalsInstaller(Container).Install();

            //Install Consumables
            new ConsumableInstaller(Container, _initialPrice).Install();

            //Install Consumed
            new ConsumedInstaller(Container).Install();

            //Install Consuming Logic
            new ConsumingLogicInstaller(Container).Install();

            //Install Releasing Logic
            new ReleasingLogicInstaller(Container).Install();

            //Install Cooking Logic
            new CookingInstaller(Container).Install();
        }
    }
}
