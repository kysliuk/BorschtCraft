using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class ItemDeliveredSignal
    {
        public CustomerDeliverySignal DeliverySignal { get; }
        public bool Delivered { get; }
        public ItemDeliveredSignal(CustomerDeliverySignal signal, bool delivered)
        {
            DeliverySignal = signal;
            Delivered = delivered;
        }
    }
}
