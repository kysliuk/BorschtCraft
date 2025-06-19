using BorschtCraft.Food;

public class CookingSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IConsumed consumed) =>
        consumed is ICookable && slot.SlotType == SlotType.Cooking;
}

public class CombiningSlotStrategy : ISlotMatchingStrategy
{
    public bool Matches(ISlot slot, IConsumed consumed) =>
        consumed is not ICookable && slot.SlotType == SlotType.Combining;
}
