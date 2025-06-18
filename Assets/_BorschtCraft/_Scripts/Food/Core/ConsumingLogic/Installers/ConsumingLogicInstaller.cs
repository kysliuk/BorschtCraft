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

            _container.Bind<IItemHandler>().FromMethod(x => 
            {
                var cookableItemHandler = x.Container.Resolve<CookableItemHandler>();
                var combiningItemHandler = x.Container.Resolve<CombiningItemHandler>();
                cookableItemHandler.SetNext(combiningItemHandler);
                return cookableItemHandler;
            }
            ).AsSingle();
        }

        public ConsumingLogicInstaller(DiContainer container) : base(container)
        {
        }
    }
}
