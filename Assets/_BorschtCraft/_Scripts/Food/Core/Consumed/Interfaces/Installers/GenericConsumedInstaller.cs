using UnityEngine;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class GenericConsumedInstaller<T> where T : IConsumed
    {
        public void Install(DiContainer container)
        {
            Logger.LogInfo(this, $"Installing binding for ConsumedViewModel for {typeof(T).Name}");

            container.Bind<ConsumedViewModel<T>>()
                .AsCached();
        }
    }
}
