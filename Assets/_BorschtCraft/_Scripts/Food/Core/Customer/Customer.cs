using ModestTree;
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

        public void SetOrderToSlot(ISlot[] slots)
        {
            var firstSlot = slots[0];
            var secondSlot = slots[1];

            if (_order.Dish != null)
            {
                var itemsToPlace = new List<IConsumed>() { _order.Dish };
                itemsToPlace.AddRange(_order.Dish.Ingredients);
                foreach (var item in itemsToPlace.AsEnumerable().Reverse())
                    firstSlot.TrySetItem(item);
            }

            if (_order.Drink != null)
                secondSlot.TrySetItem(_order.Drink as IConsumed);
        }

        public bool Satisfy(IConsumed item, out IConsumed satisfiedItem)
        {
            satisfiedItem = null;
            if (_order.MathesIngredients(item))
            {
                _receivedDish = true;
                satisfiedItem = item;
            }

            if (item is IDrink && MatchesOrder(item))
            {
                _receivedDrink = true;
                satisfiedItem = item;
            }

            return IsSatisfied();
        }

        private bool IsSatisfied()
        {
            bool needsDish = _order.Dish != null;
            bool needsDrink = _order.Drink != null;

            bool dishOk = !needsDish || _receivedDish;
            bool drinkOk = !needsDrink || _receivedDrink;

            return dishOk && drinkOk;
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
