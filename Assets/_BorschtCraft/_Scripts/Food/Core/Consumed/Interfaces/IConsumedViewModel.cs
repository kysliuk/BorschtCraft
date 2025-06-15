using System;
using UniRx;

namespace BorschtCraft.Food.UI
{
    public interface IConsumedViewModel
    {
        IReadOnlyReactiveProperty<bool> IsVisible { get; }
        void SetParentSlotViewModel(SlotViewModel slotViewModel);
    }
}