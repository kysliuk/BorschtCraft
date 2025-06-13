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

        public bool CookItemInSlot(IItemSlot slot)
        {
            if (slot == null || slot.GetCurrentItem() == null)
            {
                Logger.LogWarning(this, "Cannot cook: Slot is null or empty.");
                return false;
            }

            var itemToCook = slot.GetCurrentItem();

            if(itemToCook is ICookable cookableItem)
            {   
                _coroutineHost.StartCoroutine(PerformCookeing(cookableItem, slot));
                return true;
            }

            return false;
        }

        private void OnCookItemInSlotRequested(CookItemInSlotRequestSignal signal)
        {
            if(signal.Slot == null || signal.Slot.GetCurrentItem() == null)
            {
                Logger.LogWarning(this, "Cook request received for an invalid or empty slot.");
                return;
            }

            CookItemInSlot(signal.Slot);
        }

        private IEnumerator PerformCookeing(ICookable cookableItem, IItemSlot slot)
        {
            yield return new WaitForSeconds(cookableItem.CookingTime);

            IConsumed coockedItem = cookableItem.Cook();
            slot.TrySetItem(coockedItem);

            _signalBus.Fire(new ItemCookedSignal(coockedItem, slot));
            Logger.LogInfo(this, $"Cooked item: {coockedItem.GetType().Name} in slot: {slot.GetGameObject().name}");
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
