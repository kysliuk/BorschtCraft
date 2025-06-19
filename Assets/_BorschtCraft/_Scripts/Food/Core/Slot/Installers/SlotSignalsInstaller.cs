using Zenject;

namespace BorschtCraft.Food
{
    public class SlotSignalsInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.DeclareSignal<ReleaseSlotItemSignal>();
            _container.DeclareSignal<ClearAllViewsInSlotSignal>();
        }

        public SlotSignalsInstaller(DiContainer container) : base(container)
        {
        }
    }
}
