using Zenject;

namespace BorschtCraft.Food
{
    public class SlotSignalsInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.DeclareSignal<ReleaseSlotItemSignal>();
        }

        public SlotSignalsInstaller(DiContainer container) : base(container)
        {
        }
    }
}
