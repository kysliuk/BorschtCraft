using UnityEngine;

namespace BorschtCraft.Food
{
    public interface IItemSlot
    {
        IConsumed GetCurrentItem();
        bool TrySetItem(IConsumed newItem);
        IConsumed ReleaseItem();
        bool IsEmpty();
        GameObject GetGameObject();
    }
}
