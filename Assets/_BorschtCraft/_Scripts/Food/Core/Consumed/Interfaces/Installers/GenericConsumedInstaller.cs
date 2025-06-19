using Zenject;

namespace BorschtCraft.Food.UI
{
    public class GenericConsumedInstaller<T> : MonoInstaller where T : IConsumed
    {
        public override void InstallBindings()
        {
            Logger.LogInfo(this, $"Binding for ConsumedViewModel for {typeof(T).Name}");

            Container.Bind<ConsumedViewModel<T>>()
                .AsSingle();
        }
    }
}
