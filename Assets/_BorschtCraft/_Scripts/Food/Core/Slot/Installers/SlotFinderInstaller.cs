using Zenject;

namespace BorschtCraft.Food
{
    public class SlotFinderInstaller : InstallerBase
    {
        [Inject] private ISlot[] _slots;

        public override void Install()
        {
            _container.Bind<ISlot[]>().FromMethod(_ => _container.ResolveAll<ISlot>().ToArray()).AsCached();
            _container.QueueForInject(this);

            _container.Inject(this);

            SlotFinderHelper.Initialize(_slots);
        }

        public SlotFinderInstaller(DiContainer container) : base(container)
        {
        }
    }
}
