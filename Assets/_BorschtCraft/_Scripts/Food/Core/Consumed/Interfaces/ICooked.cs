namespace BorschtCraft.Food
{
    public interface ICooked : ICombinable
    {
        float BurningTime { get; }

        IConsumed Burn();
        bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed);
    }
}
