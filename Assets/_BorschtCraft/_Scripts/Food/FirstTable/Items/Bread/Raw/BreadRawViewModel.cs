using Zenject;

namespace BorschtCraft.Food.UI
{
    public class BreadRawViewModel : ConsumedViewModel<BreadRaw>
    {
        public BreadRawViewModel(BreadRaw consumed, SignalBus signalBus) : base(consumed, signalBus)
        { }
    }

}
