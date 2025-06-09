namespace BorschtCraft.Food.Signals
{
    public class ConsumedItemCreatedSignal<T1, T2> 
        where T1 : Consumable<T2> 
        where T2 : Consumed
    {
        public T1 ConsumableItem { get; }
        public T2 ConsumedItem { get; }

        public ConsumedItemCreatedSignal(T1 consumableItem, T2 consumedItem)
        {
            ConsumableItem = consumableItem;
            ConsumedItem = consumedItem;
        }
    }
}
