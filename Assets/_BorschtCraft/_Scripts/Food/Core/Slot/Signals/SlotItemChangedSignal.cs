using BorschtCraft.Food.UI;
using Codice.Client.BaseCommands.Merge.Xml;
using System;

namespace BorschtCraft.Food
{
    public class SlotItemChangedSignal<T> where T : IConsumed
    {
        public ISlot Slot { get; private set; }
        public SlotViewModel SlotViewModel { get; private set; }

        public SlotItemChangedSignal(ISlot slot, SlotViewModel viewModel)
        {
            Slot = slot;
            SlotViewModel = viewModel;
        }
    }
}
