namespace BorschtCraft.Food
{
    public interface ICookable<T> where T : Consumed
    {
        T Cook();
    }
}
