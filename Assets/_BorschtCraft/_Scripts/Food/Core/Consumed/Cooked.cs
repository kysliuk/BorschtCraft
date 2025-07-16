using System;

namespace BorschtCraft.Food
{
    public abstract class Cooked<T1, T2> : Consumed, ICooked 
        where T1 : Consumed, ICooked 
        where T2 : Consumed, IBurned
    {
        public float BurningTime { get; }

        public virtual IConsumed Burn() 
        {
            ConsumeAbstractFactory.CreateConsumed<T2>(0, this.WrappedItem);
            throw new InvalidOperationException("Wrapped item does not implement IBurned interface.");
        }

        public virtual bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed) 
        {
            outConsumed = ConsumeAbstractFactory.CreateConsumed<T1>(this.Price, consumed);
            return consumed == null;
        }

        public Cooked(int price, IConsumed wrappedItem) : base(price, wrappedItem)
        {
             if(wrappedItem is not ICookable cookable)
                throw new ArgumentException("Wrapped item must implement ICookable interface.", nameof(wrappedItem));

            BurningTime = cookable.CookingTime;
        }
    }
}
