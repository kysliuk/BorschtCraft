using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingLogicInstaller : InstallerBase
    {
        public ConsumingLogicInstaller(DiContainer container) : base(container) { }

        public override void Install()
        {
            // Bind both handlers to themselves and IInitializable (but NOT IItemHandler!)
            _container.Bind<CookableItemHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CookableItemHandler>().FromResolve();

            _container.Bind<CombiningItemHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CombiningItemHandler>().FromResolve();

            // Chain handlers and expose only the entrypoint as IItemHandler
            _container.Bind<IItemHandler>().FromMethod(ctx =>
            {
                var cookable = ctx.Container.Resolve<CookableItemHandler>();
                var combining = ctx.Container.Resolve<CombiningItemHandler>();
                cookable.SetNext(combining);
                return cookable;
            }).AsSingle().NonLazy();

            // Bind your consuming service
            _container.BindInterfacesAndSelfTo<ConsumingService>().AsSingle();
        }
    }
}
