using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingLogicInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.Bind<CookableItemHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CookableItemHandler>().FromResolve();

            _container.Bind<CombiningItemHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CombiningItemHandler>().FromResolve();

            _container.Bind<IConsumingItemHandler>().FromMethod(ctx =>
            {
                var cookable = ctx.Container.Resolve<CookableItemHandler>();
                var combining = ctx.Container.Resolve<CombiningItemHandler>();
                cookable.SetNext(combining);
                return cookable;
            }).AsSingle().NonLazy();

            _container.BindInterfacesAndSelfTo<ConsumingService>().AsSingle();
        }

        public ConsumingLogicInstaller(DiContainer container) : base(container) { }
    }
}
