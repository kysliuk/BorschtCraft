namespace BorschtCraft.Food
{
    public interface IConsumable : IItem
    {
        bool TryConsume(IConsumed item);
        bool TryConsume(IConsumed item, out IConsumed consumed);
        bool CanDecorate(IConsumed item);
    }
}
