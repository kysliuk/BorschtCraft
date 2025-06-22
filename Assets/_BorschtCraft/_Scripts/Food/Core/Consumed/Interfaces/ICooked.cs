namespace BorschtCraft.Food
{
    public interface ICooked
    {
        bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed);
    }
}
