using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
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
                _coroutineHost.StartCoroutine(PerformCookeing(cookableItem, slotController));
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

        private IEnumerator PerformCookeing(ICookable cookableItem, ItemSlotController slot)
        {
            yield return new WaitForSeconds(cookableItem.CookingTime);

            IConsumed coockedItem = cookableItem.Cook();
            slot.TrySetItem(coockedItem);

            _signalBus.Fire(new ItemCookedSignal(coockedItem, slot));
            Logger.LogInfo(this, $"Cooked item: {coockedItem.GetType().Name} in slot: {slot.gameObject.name}");
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
