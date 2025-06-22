using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingLogicInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.BindInterfacesAndSelfTo<CookingItemHandler>().AsSingle().NonLazy();
            _container.BindInterfacesAndSelfTo<CombiningItemHandler>().AsSingle().NonLazy();
            _container.BindInterfacesAndSelfTo<DrinkingItemHandler>().AsSingle().NonLazy();
        }

        public ConsumingLogicInstaller(DiContainer container) : base(container) { }
    }
}
