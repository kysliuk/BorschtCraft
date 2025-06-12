using Zenject;

namespace BorschtCraft.Food.UI
{
    public class BreadCookedViewModel : ConsumedViewModel<BreadCooked>
    {
        public BreadCookedViewModel(BreadCooked consumedModel, SignalBus signalBus) : base(consumedModel, signalBus)
        {
        }
    }
}
