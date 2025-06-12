using BorschtCraft.Food.UI;
using System;
using Zenject;

namespace BorschtCraft.Food
{
    public interface ISelectedItemService : IInitializable, IDisposable
    {
        ItemSlotController CurrentSelectedSlot { get; }
        IConsumed CurrentSelectedItem { get; }
        void SelectSlot(ItemSlotController slot);
        void Deselect();
    }
}
