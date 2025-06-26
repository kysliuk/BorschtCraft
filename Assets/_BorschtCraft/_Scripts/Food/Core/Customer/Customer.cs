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

        public void CleatSlot(ISlot slot)
        {
            slot.ClearCurrentItem();
        }

        public bool Satisfy(IConsumed item)
        {
            if(_order.MathesIngredients(item))
                _receivedDish = true;

            if(item is IDrink && MatchesOrder(item))
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

        public bool MatchesOrder(IConsumed item)
        {
            return item is IDrink && _order.Drink != null || _order.MathesIngredients(item);
        }

        public Customer(CustomerOrder customerOrder) 
        {
            _order = customerOrder;
        }
    }
}
