using Zenject;

namespace BorschtCraft.Food.UI
{
    public class GarlicStackViewModel : ConsumableViewModel<GarlicStack, Garlic>
    {
        public GarlicStackViewModel(GarlicStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
