using Zenject;
using BorschtCraft.Food.Signals;
using UnityEngine;

namespace BorschtCraft.Food
{
    public class ConsumableInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MonoBehaviour>().WithId("CoroutineHost").FromInstance(this).AsSingle();

            //Bind Signals
            Container.DeclareSignal<ConsumableInteractionRequestSignal>();
            Container.DeclareSignal<CookItemInSlotRequestSignal>();
            Container.DeclareSignal<ItemCookedSignal>();
            Container.DeclareSignal<SlotClickedSignal>();
        }
    }
}
