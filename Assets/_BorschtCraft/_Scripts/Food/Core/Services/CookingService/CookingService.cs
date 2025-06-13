using BorschtCraft.Food.Signals;
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

        private void OnCookItemInSlotRequested(CookItemInSlotRequestSignal signal)
        {
            CookItemInSlot(signal.Slot);
        }

        public bool CookItemInSlot(IItemSlot slot)
        {
            if (slot == null || slot.IsEmpty()) return false;

            if (slot.CurrentItem.Value is ICookable cookableItem)
            {
                _coroutineHost.StartCoroutine(PerformCooking(cookableItem, slot));
                return true;
            }
            return false;
        }

        private IEnumerator PerformCooking(ICookable cookableItem, IItemSlot slot)
        {
            yield return new WaitForSeconds(cookableItem.CookingTime);

            // Ensure the item hasn't been moved/changed while cooking
            if (slot.CurrentItem.Value == cookableItem)
            {
                IConsumed cookedItem = cookableItem.Cook();
                slot.TrySetItem(cookedItem);
                _signalBus.Fire(new ItemCookedSignal(cookedItem, slot));
                Logger.LogInfo(this, $"Cooked item: {cookedItem.GetType().Name}.");
            }
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