using BorschtCraft;
using BorschtCraft.Food;

public abstract class CookingSlotStrategy : ISlotMatchingStrategy
{
    public SlotType SlotType => SlotType.Cooking;
    public abstract bool Matches(ISlot slot, IItem item);
}

public abstract class CombiningSlotStrategy : ISlotMatchingStrategy
{
    public SlotType SlotType => SlotType.Combining;
    public abstract bool Matches(ISlot slot, IItem item);
}

public class ConsumingInCookingSlotStrategy : CookingSlotStrategy
{
    public override bool Matches(ISlot slot, IItem consumed) =>
        consumed is ICookable && slot.SlotType == SlotType.Cooking;
}

public class ConsumingInCombiningSlotStrategy : CombiningSlotStrategy
{
    public override bool Matches(ISlot slot, IItem consumed)
    {
        Logger.LogInfo(this, $"Checking if slot of type {slot.SlotType} matches combining strategy with consumed item of type {consumed.GetType().Name}.");
        return consumed is not ICookable && slot.SlotType == SlotType.Combining;
    }
}

public class ReleasingCookingSlotStrategy : CookingSlotStrategy
{
    public override bool Matches(ISlot slot, IItem consumed) =>
        consumed is ICooked;
}

public class ReleasingCombiningSlotStrategy : CombiningSlotStrategy
{
    public override bool Matches(ISlot slot, IItem consumed) =>
         true;
}
