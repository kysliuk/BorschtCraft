using BorschtCraft.Food.UI;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    [RequireComponent(typeof(CustomerMover))]
    public class CustomerController : MonoBehaviour
    {
        [SerializeField] private SlotView[] _customerSlotViews;
        private CustomerMover _mover;
        private Customer _customer;

        [Inject] private SignalBus _signalBus;

        private ISlot[] _slots => _customerSlotViews.Select(sv => sv.SlotViewModel.Slot).ToArray();

        public bool HasMatchingOrder(CustomerDeliverySignal signal)
        {
            return _customer.MatchesOrder(signal.Item);
        }

        public bool TrySatisfyOrder(CustomerDeliverySignal signal)
        {
            var satisfied = _customer.Satisfy(signal.Item, out var satisfiedItem);

            if (satisfiedItem != null)
            {
                var satisfiedSlot = FindSlotWithItem(satisfiedItem);
                satisfiedSlot?.ClearCurrentItem();
                _signalBus.Fire(new ItemDeliveredSignal(signal.DeliveryId, true));
                Logger.LogInfo(this, $"Fired signal {nameof(ItemDeliveredSignal)}. Delivery signal HashCode: {signal.GetHashCode()}");
            }
            return satisfied;
        }

        public void LeaveSatisfied()
        {
            _mover.MoveCustomerOut(() => gameObject.SetActive(false));
        }

        public void LeaveUnhappy()
        {
            LeaveSatisfied();
        }

        private void Awake()
        {
            _mover = this.gameObject.GetComponent<CustomerMover>();
        }

        private ISlot FindSlotWithItem(IConsumed item)
        {
            return _slots.Where(s => s.Item.Value != null).FirstOrDefault(s => IngredientUtils.MatchItemsIngredients(s.Item?.Value, item));
        }

        public void Construct(Customer customer)
        {
            _customer = customer;

            _mover.MoveCustomerIn(() => _customer.SetOrderToSlot(_slots));
        }
    }
}
