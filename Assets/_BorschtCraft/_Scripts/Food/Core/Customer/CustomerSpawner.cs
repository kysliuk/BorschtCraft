using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private int _ordersAmount = 5;

        [Inject] private CustomerOrderGenerator _orderGenerator;
        [Inject] private IFactory<CustomerOrder, Customer> _customerFactory;
        [Inject] private CustomerControllerPool _customerPool;

        public void SpawnCustomers()
        {
            var orders = _orderGenerator.GenerateOrders(_ordersAmount);

            foreach (var order in orders)
            {
                var customer = _customerFactory.Create(order);
                var controller = _customerPool.Spawn(customer);

                controller.transform.position = GetOffscreenSpawnPosition();
            }
        }

        public CustomerController SpawnCustomer()
        {
            var order = _orderGenerator.GenerateOrder();
            var customer = _customerFactory.Create(order);
            var controller = _customerPool.Spawn(customer);
            controller.transform.position = GetOffscreenSpawnPosition();

            return controller;
        }

        private Vector3 GetOffscreenSpawnPosition()
        {
            float y = 0.2f;
            float z = 0f; 

            float x = Random.value < 0.5f ? -0.2f : 1.2f;

            return Camera.main.ViewportToWorldPoint(new Vector3(x, y, z));
        }

    }
}

