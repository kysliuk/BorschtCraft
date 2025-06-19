using Zenject;

namespace BorschtCraft.Food
{
    public class ReleasingLogicInstaller : InstallerBase
    {
        public override void Install()
        {
            _container.Bind<CookingSlotReleasingHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CookingSlotReleasingHandler>().FromResolve();

            _container.Bind<CombiningSlotReleasingHandler>().AsSingle();
            _container.Bind<IInitializable>().To<CombiningSlotReleasingHandler>().FromResolve();

            _container.Bind<ISlotReleasingHandler>().FromMethod(ctx =>
            {
                var cooking = ctx.Container.Resolve<CookingSlotReleasingHandler>();
                var combining = ctx.Container.Resolve<CombiningSlotReleasingHandler>();
                cooking.SetNext(combining);
                return cooking;
            }).AsSingle().NonLazy();

            _container.BindInterfacesAndSelfTo<ReleasingService>().AsSingle();
        }

        public ReleasingLogicInstaller(DiContainer container) : base(container)
        {
        }
    }
}
