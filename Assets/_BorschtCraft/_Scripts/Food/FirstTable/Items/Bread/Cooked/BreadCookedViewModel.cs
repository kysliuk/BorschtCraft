using Zenject;

namespace BorschtCraft.Food.UI
{
    public class BreadCookedViewModel : ConsumedViewModel<BreadRaw>
    {
        public BreadCookedViewModel(SignalBus signalBus) : base(signalBus)
        {
        }
    }
}
