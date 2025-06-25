using BorschtCraft.Food.UI;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class SlotInstaller : MonoInstaller
    {
        [SerializeField] private SlotType _slotType;

        public override void InstallBindings()
        {
            Container.Bind<ISlot>()
            .To<Slot>()
            .FromMethod(context =>
            {
                var slot = new Slot(_slotType, null);
                var registry = context.Container.Resolve<ISlotRegistry>();
                registry.Register(slot);
                return slot;
            })
            .AsSingle()
            .NonLazy();

            Container.Bind<SlotViewModel>().AsSingle();
            Container.Bind<SlotView>().FromComponentInHierarchy().AsSingle();
        }
    }
}
