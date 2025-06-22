using BorschtCraft.Food;

public abstract class SlotMatchingStrategy : ISlotMatchingStrategy
{
    public abstract SlotType SlotType { get; }
    public virtual bool Matches(ISlot slot, IItem item) => slot.SlotType == SlotType;
}

public class CookingSlotStrategy : SlotMatchingStrategy
{
    public override SlotType SlotType => SlotType.Cooking;
}

public class CombiningSlotStrategy : SlotMatchingStrategy
{
    public override SlotType SlotType => SlotType.Combining;
}

public class DrinkingSlotStrategy : SlotMatchingStrategy
{
    public override SlotType SlotType => SlotType.Drinking;
}

public class ConsumingInCookingSlotStrategy : CookingSlotStrategy
{
    public override bool Matches(ISlot slot, IItem item) => base.Matches(slot, item) &&
        item is ICookable && slot.SlotType == SlotType.Cooking;
}

public class ConsumingInCombiningSlotStrategy : CombiningSlotStrategy
{
    public override bool Matches(ISlot slot, IItem item) => base.Matches(slot, item) && item is not ICookable && slot.SlotType == SlotType.Combining;
}

public class ReleasingCookingSlotStrategy : CookingSlotStrategy
{
    public override bool Matches(ISlot slot, IItem item) => base.Matches(slot, item) &&
        item is ICooked;
}

public class ReleasingCombiningSlotStrategy : CombiningSlotStrategy
{
    public override bool Matches(ISlot slot, IItem item) =>
         base.Matches(slot, item);
}
