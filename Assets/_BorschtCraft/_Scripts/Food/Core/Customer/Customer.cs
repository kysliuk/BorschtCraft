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
        private bool _readyToRecieve = false;

        public void SetOrderToSlot(ISlot[] slots)
        {
            var emptySlot = GetEmptySlot(slots);

            if (_order.Dish != null)
            {
                var itemsToPlace = new List<IConsumed>() { _order.Dish };
                itemsToPlace.AddRange(_order.Dish.Ingredients);
                foreach (var item in itemsToPlace.AsEnumerable().Reverse())
                    emptySlot.TrySetItem(item);
            }

            emptySlot = GetEmptySlot(slots);

            if (_order.Drink != null)
                emptySlot.TrySetItem(_order.Drink as IConsumed);

            _readyToRecieve = true;
        }

        public bool Satisfy(IConsumed item, out IConsumed satisfiedItem)
        {
            satisfiedItem = null;

            if (!_readyToRecieve)
                return false;   

            if (_order.MathesIngredients(item) && !_receivedDish)
            {
                _receivedDish = true;
                satisfiedItem = item;
            }

            if (item is IDrink && MatchesOrder(item) && !_receivedDrink)
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

        private ISlot GetEmptySlot(ISlot[] slots)
        {
            var emptySlot = slots.FirstOrDefault(s => s.Item.Value == null);
            return emptySlot ?? throw new ArgumentNullException(nameof(emptySlot));
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
