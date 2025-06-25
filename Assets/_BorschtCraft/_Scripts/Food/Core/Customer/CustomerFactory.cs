using Zenject;

namespace BorschtCraft.Food
{
    public class CustomerFactory : IFactory<CustomerOrder, Customer>
    {
        public Customer Create(CustomerOrder order)
        {
            return new Customer(order);
        }
    }
}
