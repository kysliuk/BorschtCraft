using BorschtCraft.Food.UI;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    [RequireComponent(typeof(CustomerMover))]
    public class CustomerController : MonoBehaviour
    {
        [SerializeField] private SlotView _customerSlotView;
        private CustomerMover _mover;
        private Customer _customer;

        private ISlot _slot => _customerSlotView.SlotViewModel.Slot;

        public bool HasMatchingOrder(CustomerDeliverySignal signal)
        {
            return _customer.MatchesOrder(signal.Item);
        }

        public bool TrySatisfyOrder(CustomerDeliverySignal signal)
        {
            return _customer.Satisfy(signal.Item);
        }

        public void LeaveSatisfied()
        {
            _customer.CleatSlot(_slot);
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

        public void Construct(Customer customer)
        {
            _customer = customer;

            _mover.MoveCustomerIn(() => _customer.SetOrderToSlot(_slot));
        }
    }
}
