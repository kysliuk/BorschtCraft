namespace BorschtCraft.Food
{
    public interface IConsumable : IConsumed
    {
        IConsumed Consume(IConsumed item);
    }
}
