using BorschtCraft.Food.UI;
using System;
using Zenject;

namespace BorschtCraft.Food
{
    public interface ICookingService : IInitializable, IDisposable
    {
        bool CookItemInSlot(ItemSlotController slotController);
    }
}
