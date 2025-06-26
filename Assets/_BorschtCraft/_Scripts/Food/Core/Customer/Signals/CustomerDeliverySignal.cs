using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class CustomerDeliverySignal
    {
        public IReadOnlyCollection<IConsumed> Ingredients;
        public IDrink Drink;
    }
}
