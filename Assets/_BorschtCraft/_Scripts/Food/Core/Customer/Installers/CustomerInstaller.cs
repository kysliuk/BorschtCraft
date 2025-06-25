using BorschtCraft.Food.FirstTable;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class CustomerInstaller : MonoInstaller
    {
        [SerializeField] private CustomerController _customerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ITableIngredientsList>()
            .To<TableIngredientsList>()
            .AsSingle();
            Container.Bind<CustomerOrderGenerator>().AsSingle();
            Container.Bind<CustomerSpawner>().FromComponentInHierarchy().AsSingle();
            Container.BindIFactory<CustomerOrder, Customer>().To<Customer>().AsTransient();

            Container.BindMemoryPool<CustomerController, CustomerControllerPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_customerPrefab)
                .UnderTransformGroup(nameof(CustomerControllerPool));
        }
    }

}
