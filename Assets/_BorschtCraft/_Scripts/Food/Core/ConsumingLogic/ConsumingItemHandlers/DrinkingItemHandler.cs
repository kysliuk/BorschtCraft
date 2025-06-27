using System;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace BorschtCraft.Food
{
    public class DrinkingItemHandler : ConsumingItemHandlerBase<DrinkingSlotStrategy>
    {
        private readonly ReplaySubject<GlassFilledSignal> _glassFilledSubject = new(1);

        private Action<GlassFilledSignal> _onGlassFilled;

        protected override async Task<bool> Process(IItem item)
        {
            var anyEmptyDrinkingSlot = item is Consumable<Drink> && _slotRegistry.Slots.Where(s => s.SlotType == _strategy.SlotType && s.Item.Value == null).Any();
            
            if (!anyEmptyDrinkingSlot)
                return false;

            var fillGlassSignal = new FillGlassSignal();
            _signalBus.Fire(fillGlassSignal);

            var glassFilledResult = await _glassFilledSubject
                .First(sigal => sigal.FillingId == fillGlassSignal.FillingId)
                .ToTask();


            return await base.Process(item);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _onGlassFilled = signal => _glassFilledSubject.OnNext(signal);
            _signalBus.Subscribe(_onGlassFilled);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _signalBus.TryUnsubscribe(_onGlassFilled);
        }
    }
}
