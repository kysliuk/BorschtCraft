using Zenject;

namespace BorschtCraft.Food.UI
{
    public class MustardStackViewModel : ConsumableViewModel<MustardStack, Mustard>
    {
        public MustardStackViewModel(MustardStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
