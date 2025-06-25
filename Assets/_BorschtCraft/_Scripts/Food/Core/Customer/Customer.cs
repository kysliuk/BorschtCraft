namespace BorschtCraft.Food
{
    public class Customer
    {
        protected CustomerOrder _order { get; private set; }

        public void SetOrderToSlot(ISlot slot)
        {
            foreach (var item in _order.Ingredients)
                slot.TrySetItem(item);

            if (_order.Drink != null)
                slot.TrySetItem(_order.Drink as IConsumed);
        }

        public Customer(CustomerOrder customerOrder) 
        {
            _order = customerOrder;
        }
    }
}
