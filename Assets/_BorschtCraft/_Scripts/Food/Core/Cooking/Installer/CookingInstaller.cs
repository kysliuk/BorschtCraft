using Zenject;

namespace BorschtCraft.Food
{
    public class CookingInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.DeclareSignal<CookItemInSlotSignal>();
            _container.DeclareSignal<ItemCookedSignal>();

            _container.BindInterfacesAndSelfTo<CookingService>()
                .AsSingle()
                .NonLazy();
        }

        public CookingInstaller(DiContainer container) : base(container)
        {
        }
    }
}
