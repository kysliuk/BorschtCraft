using System;

namespace BorschtCraft.Food
{
    public class ItemDeliveredSignal
    {
        public Guid DeliveryId { get; }
        public bool Delivered { get; }
        public ItemDeliveredSignal(Guid deliveryId, bool delivered)
        {
            DeliveryId = deliveryId;
            Delivered = delivered;
        }
    }
}
