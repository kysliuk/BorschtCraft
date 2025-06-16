using Zenject;
using UnityEngine;

namespace BorschtCraft.Food.FirstTable
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private int _initialPrice = 10; //To be changed with gameconfig
        public override void InstallBindings()
        {
            //Install Consumables
            new ConsumableInstaller(Container, _initialPrice).Install();

            //Install Consumed
            new ConsumedInstaller(Container).Install();
        }
    }
}
