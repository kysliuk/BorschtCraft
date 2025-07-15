using BorschtCraft.Food.UI;
using System;

namespace BorschtCraft.Food
{
    public class CustomerDeliverySignal
    {
        public Guid DeliveryId { get; }
        public IConsumed Item;
        public SlotView SlotView;

        public CustomerDeliverySignal(IConsumed item, SlotView slotView)
        {
            Item = item;
            SlotView = slotView;
            DeliveryId = Guid.NewGuid();
        }
    }
}
