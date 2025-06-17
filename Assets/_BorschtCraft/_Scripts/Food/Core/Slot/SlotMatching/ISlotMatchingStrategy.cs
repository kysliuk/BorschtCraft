using BorschtCraft.Food;

public interface ISlotMatchingStrategy
{
    bool Matches(ISlot slot, IConsumed consumed);
}
