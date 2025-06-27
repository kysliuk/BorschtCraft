using System;

namespace BorschtCraft.Food
{
    public class CustomerDeliverySignal
    {
        public Guid DeliveryId { get; }
        public IConsumed Item;

        public CustomerDeliverySignal(IConsumed item)
        {
            Item = item;
            DeliveryId = new Guid();
        }
    }
}
