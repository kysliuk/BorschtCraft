using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class CookingService : ICookingService
    {
        protected readonly SignalBus _signalBus;
        protected readonly MonoBehaviour _coroutineHost;

        public void Initialize()
        {
            _signalBus.Subscribe<CookItemInSlotSignal>(OnCookItemInSlotSignal);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CookItemInSlotSignal>(OnCookItemInSlotSignal);
        }

        private void OnCookItemInSlotSignal(CookItemInSlotSignal signal)
        {
            if(signal.Slot == null || signal.Slot?.Item?.Value == null)
            {
                Logger.LogWarning(this, "Cook request received for an invalid or empty slot.");
                return;
            }

            CookItemInSlot(signal.Slot);
        }

        private void CookItemInSlot(ISlot slot)
        {
            if(slot.Item.Value is ICookable cookable)
            {
                Logger.LogInfo(this, $"Cooking item: {cookable.GetType().Name} in slot: {slot.GetHashCode()}");
                _coroutineHost.StartCoroutine(ProcessCooking(cookable, slot));
            }
            else
            {
                Logger.LogWarning(this, $"Item in slot {slot.GetHashCode()} is not cookable but {slot.Item.Value.GetType().Name}.");
            }
        }

        private IEnumerator ProcessCooking(ICookable cookable, ISlot slot)
        {
            yield return new WaitForSeconds(cookable.CookingTime);

            var cookedItem = cookable.Cook();

            slot.ClearCurrentItem();
            Logger.LogInfo(this, "Clearing slot for next set");
            slot.TrySetItem(cookedItem);
        }

        public CookingService(SignalBus signalBus, [Inject(Id = "CoroutineHost", Optional = true)] MonoBehaviour coroutineHost)
        {
            _signalBus = signalBus;
            _coroutineHost = coroutineHost;
        }
    }
}
