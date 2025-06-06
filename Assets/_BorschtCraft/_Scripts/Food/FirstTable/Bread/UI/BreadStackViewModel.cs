using Zenject;

namespace BorschtCraft.Food.UI
{
    public class BreadStackViewModel : ConsumableViewModel<BreadStack, BreadRaw>
    {
        public BreadStackViewModel(BreadStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
