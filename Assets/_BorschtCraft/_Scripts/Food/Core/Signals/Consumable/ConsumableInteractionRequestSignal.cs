namespace BorschtCraft.Food.Signals
{
    public class ConsumableInteractionRequestSignal<T1, T2> : IConsumableInteractionRequestSignal
        where T1 : Consumable<T2>
        where T2 : Consumed
    {
        public T1 ConsumableSourceConcrete { get; }

        IConsumable IConsumableInteractionRequestSignal.ConsumableSource => ConsumableSourceConcrete;

        public ConsumableInteractionRequestSignal(T1 consumableSource)
        {
            ConsumableSourceConcrete = consumableSource;
            Logger.LogInfo(this, $"Created. {consumableSource.GetType().Name} requested interaction");
        }
    }
}