namespace BorschtCraft.Food
{
    public interface ICookable
    {
        float CookingTime { get; set; }
        IConsumed Cook();
    }
}
