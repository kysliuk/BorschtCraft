using System;

namespace BorschtCraft.Food
{
    public class ItemDeliveredSignal
    {
        public Guid DeliveryId { get; }
        public ISlot Slot { get; }
        public bool Delivered { get; }
        public ItemDeliveredSignal(Guid deliveryId, ISlot slot, bool delivered)
        {
            DeliveryId = deliveryId;
            Slot = slot;
            Delivered = delivered;
        }
    }
}
