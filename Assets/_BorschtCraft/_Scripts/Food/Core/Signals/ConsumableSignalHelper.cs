namespace BorschtCraft.Food.Singnals
{
    public static class ConsumableSignalHelper
    {
        public static ConsumableInteractionRequestSignal<T1, T2> ConsumableInteractionRequestSignal<T1, T2>(T1 consumable)
        where T1 : Consumable<T2>
        where T2 : Consumed
        {
            return new ConsumableInteractionRequestSignal<T1, T2>(consumable);
        }

        public static ConsumedItemCreatedSignal<T1, T2> ConsumedItemCreatedSignal<T1, T2>(T1 consumableItem, T2 consumedItem)
        where T1 : Consumable<T2>
        where T2 : Consumed
        {
            return new ConsumedItemCreatedSignal<T1, T2>(consumableItem, consumedItem);
        }
    }
}
