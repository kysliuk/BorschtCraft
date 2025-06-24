namespace BorschtCraft.Food
{
    public interface ICooked : ICombinable
    {
        bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed);
    }
}
