namespace BorschtCraft.Food
{
    public abstract class Cooked<T> : Consumed, ICooked where T : Consumed, ICooked
    {
        public virtual bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed) 
        {
            outConsumed = ConsumeAbstractFactory.CreateConsumed<T>(this.Price, consumed);
            return consumed == null;
        }

        public Cooked(int price, IConsumed wrappedItem) : base(price, wrappedItem)
        {
        }
    }
}
