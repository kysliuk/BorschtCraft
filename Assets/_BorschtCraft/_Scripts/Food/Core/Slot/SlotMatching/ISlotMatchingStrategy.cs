using BorschtCraft.Food;

public interface ISlotMatchingStrategy
{
    SlotType SlotType { get; }
    bool Matches(ISlot slot, IItem item);
}
