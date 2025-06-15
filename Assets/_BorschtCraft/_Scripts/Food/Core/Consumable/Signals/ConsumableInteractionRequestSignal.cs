namespace BorschtCraft.Food.Signals
{
    public class ConsumableInteractionRequestSignal
    {
        public IConsumable ConsumableSource { get; }

        public ConsumableInteractionRequestSignal(IConsumable consumableSource)
        {
            ConsumableSource = consumableSource;
            Logger.LogInfo(this, $"Created. {consumableSource.GetType().Name} requested interaction");
        }
    }
}