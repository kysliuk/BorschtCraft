namespace BorschtCraft.Food.Signals
{
    public class ConsumableInteractionRequestSignal<T1, T2>
        where T1 : Consumable<T2>
        where T2 : Consumed
    {
        public T1 ConsumableSource { get; }

        public ConsumableInteractionRequestSignal(T1 consumableSource)
        {
            ConsumableSource = consumableSource;
            Logger.LogInfo(this, $"Created. {consumableSource.GetType().Name} requested interaction");
        }
    }
}