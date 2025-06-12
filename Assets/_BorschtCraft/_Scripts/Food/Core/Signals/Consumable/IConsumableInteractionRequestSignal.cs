namespace BorschtCraft.Food.Signals
{
    public interface IConsumableInteractionRequestSignal
    {
        IConsumable ConsumableSource { get; }
    }
}
