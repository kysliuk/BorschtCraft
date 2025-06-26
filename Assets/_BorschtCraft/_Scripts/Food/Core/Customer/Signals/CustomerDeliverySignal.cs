using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class CustomerDeliverySignal
    {
        public IConsumed Item;

        public CustomerDeliverySignal(IConsumed item)
        {
            Item = item;
        }
    }
}
