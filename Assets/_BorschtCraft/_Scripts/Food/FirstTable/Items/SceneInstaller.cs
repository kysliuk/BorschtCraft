using Zenject;
using UnityEngine;

namespace BorschtCraft.Food.FirstTable
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private int _initialPrice = 10; //To be changed with levelconfig
        public override void InstallBindings()
        {
            //Install Slots
            new SlotHolderInstaller(Container).Install();

            //Install Consumables
            new ConsumableInstaller(Container, _initialPrice).Install();

            //Install Consumed
            new ConsumedInstaller(Container).Install();
        }
    }
}
