using Zenject;

namespace BorschtCraft.Food.UI
{
    public class SaloStackViewModel : ConsumableViewModel<SaloStack, Salo>
    {
        public SaloStackViewModel(SaloStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
