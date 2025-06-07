using Zenject;

namespace BorschtCraft.Food.UI
{
    public class OnionStackViewModel : ConsumableViewModel<OnionStack, Onion>
    {
        public OnionStackViewModel(OnionStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
