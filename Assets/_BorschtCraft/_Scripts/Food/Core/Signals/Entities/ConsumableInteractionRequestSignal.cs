namespace BorschtCraft.Food.Singnals
{
    public class ConsumableInteractionRequestSignal<T1, T2> where T1 : Consumable<T2> where T2 : Consumed
    {
        public T1 ConsumableModel { get; }

        public ConsumableInteractionRequestSignal(T1 consumable) 
        {
            ConsumableModel = consumable;
        }
    }
}
