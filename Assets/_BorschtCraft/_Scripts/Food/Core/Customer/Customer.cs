using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BorschtCraft.Food
{
    public class Customer
    {
        protected CustomerOrder _order { get; private set; }
        private bool _receivedDish;
        private bool _receivedDrink;

        public void SetOrderToSlot(ISlot slot)
        {
            foreach (var item in _order.Ingredients)
                slot.TrySetItem(item);

            if (_order.Drink != null)
                slot.TrySetItem(_order.Drink as IConsumed);
        }

        public bool Satisfy(IReadOnlyCollection<IConsumed> ingredients, IDrink drink)
        {
            if(ingredients != null && _order.MathesIngredients(ingredients))
                _receivedDish = true;

            if(drink != null)
                _receivedDrink = true;

            return IsSatisfied();
        }

        private bool IsSatisfied()
        {
            bool needsDish = _order.Ingredients.Any();
            bool needsDrink = _order.Drink != null;

            return (!_receivedDish && !needsDish ? false : true)
            && (!_receivedDrink && !needsDrink ? false : true);
        }

        public bool IsStillWaiting()
        {
            return !IsSatisfied();
        }

        public bool MatchesOrder(IReadOnlyCollection<IConsumed> ingredients, IDrink drink)
        {
            return _order.Drink != null && drink != null ||
                   (_order.Ingredients != null && ingredients != null && _order.MathesIngredients(ingredients));
        }

        public Customer(CustomerOrder customerOrder) 
        {
            _order = customerOrder;
        }
    }
}
