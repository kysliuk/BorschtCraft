using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingLogicInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.BindInterfacesAndSelfTo<ConsumingService>()
                .AsSingle();

            _container.Bind<CookableItemHandler>().AsSingle();
            _container.Bind<CombiningItemHandler>().AsSingle();

            _container.Bind(typeof(IItemHandler), typeof(IInitializable))
                .To<CookableItemHandler>()
                .AsSingle();
        }

        public ConsumingLogicInstaller(DiContainer container) : base(container)
        {
        }
    }
}
