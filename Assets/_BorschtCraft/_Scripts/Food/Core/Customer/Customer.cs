namespace BorschtCraft.Food
{
    public class Customer
    {
        public CustomerOrder Order { get; private set; }

        public Customer(CustomerOrder customerOrder) 
        {
            Order = customerOrder;
        }
    }
}
