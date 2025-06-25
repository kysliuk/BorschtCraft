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

        private void Awake()
        {
            _mover = this.gameObject.GetComponent<CustomerMover>();
        }

        private void SetCustomerOrderToSlotView()
        {
            var slot = _customerSlotView.SlotViewModel.Slot;
            _customer.SetOrderToSlot(slot);
        }

        public void Construct(Customer customer)
        {
            _customer = customer;

            _mover.MoveCustomerIn(() => SetCustomerOrderToSlotView());
        }
    }
}
