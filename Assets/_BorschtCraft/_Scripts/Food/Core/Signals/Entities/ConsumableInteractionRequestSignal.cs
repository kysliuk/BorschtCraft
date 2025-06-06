namespace BorschtCraft.Food.Singnals
{
    public class ConsumableInteractionRequestSignal<T1, T2>
        where T1 : Consumable<T2>
        where T2 : Consumed
    {
        public T1 ConsumableSource { get; }
        public T2 TargetItem { get; }

        public ConsumableInteractionRequestSignal(T1 consumableSource, T2 targetItem = null)
        {
            ConsumableSource = consumableSource;
            TargetItem = targetItem;
            Logger.LogInfo(this, $"Created. {consumableSource.GetType().Name} requested interaction with {targetItem?.GetType().Name ?? "nothing"}");
        }
    }
}