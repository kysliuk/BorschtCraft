using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class DrinkMachineView : ConsumableView<DrinkMachine, Drink>
    {
        private SignalBus _signalBus;

        public override void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire<FillGlassSignal>();
        }

        private void OnGlassFilled(GlassFilledSignal signal)
        {
            _consumableViewModel.AttemptConsume();
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<GlassFilledSignal>(OnGlassFilled);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<GlassFilledSignal>(OnGlassFilled);
        }
    }
}
