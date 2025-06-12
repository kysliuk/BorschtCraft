using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace BorschtCraft.Food
{
    public class CookingService : ICookingService
    {
        private readonly SignalBus _signalBus;

        public void Initialize()
        {
            _signalBus.Subscribe<CookItemInSlotRequestSignal>(OnCookItemInSlotRequested);
        }

        public bool CookItemInSlot(ItemSlotController slotController)
        {
            if (slotController == null || slotController.CurrentItemInSlot == null)
            {
                Logger.LogWarning(this, "Cannot cook: Slot is null or empty.");
                return false;
            }

            var itemToCook = slotController.CurrentItemInSlot;

            if(itemToCook is ICookable cookableItem)
            {
                IConsumed coockedItem = cookableItem.Cook();
                slotController.TrySetItem(coockedItem);

                _signalBus.Fire(new ItemCookedSignal(coockedItem, slotController));
                return true;
            }

            return false;
        }

        private void OnCookItemInSlotRequested(CookItemInSlotRequestSignal signal)
        {
            if(signal.SlotController == null || signal.SlotController.CurrentItemInSlot == null)
            {
                Logger.LogWarning(this, "Cook request received for an invalid or empty slot.");
                return;
            }

            CookItemInSlot(signal.SlotController);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CookItemInSlotRequestSignal>(OnCookItemInSlotRequested);
        }

        public CookingService(SignalBus signalBus, [Inject(Id = "CookingSlots", Optional = true)] ItemSlotController[] cookingSlots)
        {
            _signalBus = signalBus;
        }
    }
}
