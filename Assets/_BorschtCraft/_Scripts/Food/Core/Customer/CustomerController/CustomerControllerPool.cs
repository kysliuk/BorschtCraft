using Zenject;

namespace BorschtCraft.Food
{
    public class CustomerControllerPool : MonoMemoryPool<Customer, CustomerController>
    {
        protected override void Reinitialize(Customer customer, CustomerController controller)
        {
            controller.Construct(customer);
            controller.gameObject.SetActive(true);
        }
    }

}
