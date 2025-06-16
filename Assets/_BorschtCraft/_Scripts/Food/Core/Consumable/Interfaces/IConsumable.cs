namespace BorschtCraft.Food
{
    public interface IConsumable
    {
        bool TryConsume(IConsumed item);
        bool TryConsume(IConsumed item, out IConsumed consumed);
        bool CanDecorate(IConsumed item);
    }
}
