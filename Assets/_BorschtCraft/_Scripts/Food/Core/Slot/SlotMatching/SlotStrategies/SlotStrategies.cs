using BorschtCraft.Food;

public class CookingSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IItem consumed) =>
        consumed is ICookable && slot.SlotType == SlotType.Cooking;
}

public class CombiningSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IItem consumed) =>
        consumed is not ICookable && slot.SlotType == SlotType.Combining;
}

public class ReleasingCookingSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IItem consumed) =>
        consumed is ICooked && slot.SlotType == SlotType.Cooking;
}

public class ReleasingCombiningSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IItem consumed) =>
         slot.SlotType == SlotType.Combining;
}
