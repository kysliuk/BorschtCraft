namespace BorschtCraft.Food
{
    public class SlotsInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.Bind<ISlot[]>().FromResolveAll().AsSingle();
        }

        public SlotsInstaller(Zenject.DiContainer container) : base(container)
        {
        }
    }
}
