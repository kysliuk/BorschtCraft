namespace BorschtCraft.Food
{
    public interface ITransferableReceiversRegistry
    {
        ITransferableReceiver[] Receivers { get; }
        void Register(ITransferableReceiver slot);
        void Clear();
    }
}
