using Zenject;

namespace BorschtCraft.Food
{
    public class ReleasingLogicInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.BindInterfacesAndSelfTo<CookingSlotReleasingHandler>().AsSingle().NonLazy();
            _container.BindInterfacesAndSelfTo<CombiningSlotReleasingHandler>().AsSingle().NonLazy();
        }

        public ReleasingLogicInstaller(DiContainer container) : base(container)
        {
        }
    }
}
