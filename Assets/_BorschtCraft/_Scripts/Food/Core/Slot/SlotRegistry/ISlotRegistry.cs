namespace BorschtCraft.Food
{
    public interface ISlotRegistry
    {
        ISlot[] Slots { get; }
        void Register(ISlot slot);
        void Clear();
    }
}
