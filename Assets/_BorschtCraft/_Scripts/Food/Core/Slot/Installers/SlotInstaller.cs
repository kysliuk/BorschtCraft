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
                var signalBus = context.Container.Resolve<SignalBus>();
                return new Slot(_slotType, null, signalBus);
            })
            .AsTransient();

            Container.Bind<SlotViewModel>().AsTransient();
        }
    }
}
