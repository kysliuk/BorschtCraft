using BorschtCraft.Food.UI;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ConsumablePrefabInstallerBase<T1, T2, T3> : MonoInstaller where T1 : ConsumableViewModel<T2, T3> where T2 : Consumable<T3> where T3 : Consumed
    {
        [Tooltip("The initial price for creating the consumable model. This will be used by the factory to create the model.")]
        [SerializeField] protected int _initialPrice = 10;

        public override void InstallBindings()
        {
            Logger.LogInfo(this, $"Installing bindings for {typeof(T1).Name} and {typeof(T2).Name}");
            Container.Bind<T2>()
                .FromMethod(model => ConsumeAbstractFactory.CreateConsumable<T2>(_initialPrice))
                .AsCached();

            Container.Bind<T1>().AsCached();
        }
    }
}
