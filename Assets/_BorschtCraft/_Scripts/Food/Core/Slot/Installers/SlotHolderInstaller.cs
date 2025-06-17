using Zenject;

namespace BorschtCraft.Food
{
    public class SlotHolderInstaller : InstallerBase
    {
        [Inject] private ISlot[] _slots;

        public override void Install()
        {
            _container.Bind<ISlot[]>().FromMethod(_ => _container.ResolveAll<ISlot>().ToArray()).AsCached();
            _container.QueueForInject(this);

            _container.Inject(this);

            SlotHolder.Initialize(_slots);
        }

        public SlotHolderInstaller(DiContainer container) : base(container)
        {
        }
    }
}
