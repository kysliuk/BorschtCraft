namespace BorschtCraft.Food
{
    public interface IConsumable
    {
        IConsumed TryConsume(IConsumed item, out bool succeed);
        bool CanDecorate(IConsumed item);
    }
}
