using System;
using Zenject;

namespace BorschtCraft.Food
{
    public interface ICookingService : IInitializable, IDisposable
    {
        bool CookItemInSlot(IItemSlot slot); // Changed parameter type
    }
}
