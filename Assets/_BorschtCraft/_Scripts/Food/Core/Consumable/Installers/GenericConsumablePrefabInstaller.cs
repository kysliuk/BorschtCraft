using UnityEngine;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class GenericConsumableInstaller<T1, T2>  where T1 : IConsumable where T2 : IConsumed
    {
        public void Install(DiContainer container, int price)
        {
            var consumableType = typeof(T1);
            var consumedType = typeof(T2);

            if (consumableType == null || consumedType == null)
            {
                Debug.LogError("Could not resolve types from names.");
                return;
            }

            var viewModelType = typeof(ConsumableViewModel<,>).MakeGenericType(consumableType, consumedType);

            Logger.LogInfo(this, $"Installing bindings for ConsumableViewModel {consumableType.Name} and {consumedType.Name}");

            var method = typeof(GenericConsumableInstaller<T1, T2>)
                .GetMethod(nameof(InstallGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.MakeGenericMethod(viewModelType, consumableType, consumedType);

            method?.Invoke(this, new object[] { price, container });
        }

        private void InstallGeneric<TViewModel, TConsumable, TConsumed>(int initialPrice, DiContainer diContainer)
            where TViewModel : ConsumableViewModel<TConsumable, TConsumed>
            where TConsumable : Consumable<TConsumed>
            where TConsumed : Consumed
        {
            diContainer.Bind<TConsumable>()
                .FromMethod(_ => ConsumeAbstractFactory.CreateConsumable<TConsumable>(initialPrice))
                .AsCached();

            diContainer.Bind<TViewModel>().AsCached();
        }
    }
}
