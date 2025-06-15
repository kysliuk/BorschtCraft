namespace BorschtCraft.Food
{
    public interface ICookable
    {
        float CookingTime { get; }
        IConsumed Cook();
    }
}
