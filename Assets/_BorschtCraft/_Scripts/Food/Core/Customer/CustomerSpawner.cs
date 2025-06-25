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

        private async void Start()
        {
            await UniTask.Delay(1000);
            SpawnCustomers();
        }

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

        private Vector3 GetOffscreenSpawnPosition()
        {
            float y = 0.2f;
            float z = 0f; 

            float x = Random.value < 0.5f ? -0.2f : 1.2f;

            return Camera.main.ViewportToWorldPoint(new Vector3(x, y, z));
        }

    }
}

