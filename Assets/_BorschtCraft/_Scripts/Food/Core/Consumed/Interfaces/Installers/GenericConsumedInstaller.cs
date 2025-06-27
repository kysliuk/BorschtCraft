using Zenject;

namespace BorschtCraft.Food.UI
{
    public class GenericConsumedInstaller<T> : MonoInstaller where T : IConsumed
    {
        public override void InstallBindings()
        {
            Container.Bind<ConsumedViewModel<T>>()
                .AsSingle();
        }
    }
}
