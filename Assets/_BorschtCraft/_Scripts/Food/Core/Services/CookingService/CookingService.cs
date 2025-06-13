using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI; // May still be needed if ItemSlotController is used elsewhere, or for casting if necessary.
using BorschtCraft.Food.Core.Interfaces; // Added
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class CookingService : ICookingService
    {
        private readonly SignalBus _signalBus;
        private readonly MonoBehaviour _coroutineHost;

        public void Initialize()
        {
            _signalBus.Subscribe<CookItemInSlotRequestSignal>(OnCookItemInSlotRequested);
        }

        public bool CookItemInSlot(IItemSlot slot) // Changed parameter type
        {
            if (slot == null || slot.GetCurrentItem() == null) // Changed access
            {
                Logger.LogWarning(this, "Cannot cook: Slot is null or empty.");
                return false;
            }

            var itemToCook = slot.GetCurrentItem(); // Changed access

            if(itemToCook is ICookable cookableItem)
            {   
                _coroutineHost.StartCoroutine(PerformCookeing(cookableItem, slot)); // Pass IItemSlot
                return true;
            }

            return false;
        }

        private void OnCookItemInSlotRequested(CookItemInSlotRequestSignal signal)
        {
            // Access signal.Slot (which is IItemSlot)
            if(signal.Slot == null || signal.Slot.GetCurrentItem() == null) // Changed access
            {
                Logger.LogWarning(this, "Cook request received for an invalid or empty slot.");
                return;
            }

            CookItemInSlot(signal.Slot); // Pass IItemSlot
        }

        private IEnumerator PerformCookeing(ICookable cookableItem, IItemSlot slot) // Changed parameter type
        {
            yield return new WaitForSeconds(cookableItem.CookingTime);

            IConsumed coockedItem = cookableItem.Cook();
            slot.TrySetItem(coockedItem); // TrySetItem is on IItemSlot

            _signalBus.Fire(new ItemCookedSignal(coockedItem, slot)); // Pass IItemSlot
            Logger.LogInfo(this, $"Cooked item: {coockedItem.GetType().Name} in slot: {slot.GetGameObject().name}"); // Changed access
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CookItemInSlotRequestSignal>(OnCookItemInSlotRequested);
        }

        public CookingService(SignalBus signalBus, [Inject(Id = "CoroutineHost", Optional = true)] MonoBehaviour coroutineHost)
        {
            _signalBus = signalBus;
            _coroutineHost = coroutineHost;
            if (_coroutineHost == null)
            {
                Logger.LogError(this, "CoroutineHost MonoBehaviour was not injected into CookingService. Cooking timers will not work.");
            }
        }
    }
}
