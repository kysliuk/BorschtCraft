namespace BorschtCraft.Food
{
    public interface IConsumable
    {
        IConsumed Consume(IConsumed item);
        bool CanDecorate(IConsumed item);
    }
}
