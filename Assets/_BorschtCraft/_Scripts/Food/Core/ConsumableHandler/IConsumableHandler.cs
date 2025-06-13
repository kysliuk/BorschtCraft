namespace BorschtCraft.Food.Handlers
{
    public interface IConsumableHandler
    {
        void SetNext(IConsumableHandler next);
        bool Handle(IConsumable consumable);
    }
}