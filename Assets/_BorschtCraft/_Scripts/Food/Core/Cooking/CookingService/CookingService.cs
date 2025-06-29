using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BorschtCraft.Food
{
    public class CookingService : ICookingService
    {
        protected readonly SignalBus _signalBus;
        protected readonly MonoBehaviour _coroutineHost;
        protected Dictionary<Coroutine, ISlot> _slots = new();

        public void Initialize()
        {
            _signalBus.Subscribe<CookItemInSlotSignal>(OnCookItemInSlotSignal);
            _signalBus.Subscribe<StopCookinItemInSlotSignal>(OnStopCookingItemInSlotSignal);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CookItemInSlotSignal>(OnCookItemInSlotSignal);
            _signalBus.TryUnsubscribe<StopCookinItemInSlotSignal>(OnStopCookingItemInSlotSignal);
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
                var coroutine = _coroutineHost.StartCoroutine(ProcessCooking(cookable, slot));
                _slots.Add(coroutine, slot);
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
            RemoveSlot(slot, out _);
        }
         
        private void OnStopCookingItemInSlotSignal(StopCookinItemInSlotSignal signal)
        {
            if (signal.Slot == null || !_slots.Values.Contains(signal.Slot))
                return;

            RemoveSlot(signal.Slot, out var coroutine);
            _coroutineHost.StopCoroutine(coroutine);
        }

        private void RemoveSlot(ISlot slot, out Coroutine coroutine)
        {
            var slotRoutine = _slots.Where(s => s.Value == slot).FirstOrDefault();
            coroutine = slotRoutine.Key;
            _slots?.Remove(slotRoutine.Key);
        }

        public CookingService(SignalBus signalBus, [Inject(Id = "CoroutineHost", Optional = true)] MonoBehaviour coroutineHost)
        {
            _signalBus = signalBus;
            _coroutineHost = coroutineHost;
        }
    }
}
