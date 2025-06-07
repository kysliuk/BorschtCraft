using Zenject;

namespace BorschtCraft.Food.UI
{
    public class HorseradishStackViewModel : ConsumableViewModel<HorseradishStack, Horseradish>
    {
        public HorseradishStackViewModel(HorseradishStack consumable, SignalBus signalBus) : base(consumable, signalBus)
        {
        }
    }
}
