namespace BorschtCraft.Food
{
    public interface IItemHandler
    {
        void SetNext(IItemHandler nextHandler);
        bool Handle(IItem item);
    }
}
