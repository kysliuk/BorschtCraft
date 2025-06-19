namespace BorschtCraft.Food
{
    public interface ITransferHandler
    {
        void SetNext(ITransferHandler nextHandler);
        bool Handle(IItem item);
    }
}
