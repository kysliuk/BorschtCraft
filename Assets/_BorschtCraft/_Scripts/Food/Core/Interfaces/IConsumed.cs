namespace BorschtCraft.Food
{
    public interface IConsumed
    {
        IConsumed WrappedItem { get; }
        int Price { get; }
    }
}
