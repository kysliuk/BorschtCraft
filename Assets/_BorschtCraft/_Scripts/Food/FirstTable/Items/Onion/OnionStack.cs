namespace BorschtCraft.Food
{
    public class OnionStack : Consumable<Onion>
    {
        public override IConsumed Consume(IConsumed item)
        {
            if (item.GetType() != typeof(BreadCooked))
            {
                Logger.LogError(this, $"cannot be consumed for {item.GetType().Name}. Only {nameof(BreadCooked)} is allowed.");
                return item;
            }

            return base.Consume(item);
        }
        public OnionStack(int price) : base(price)
        {
        }
    }
}
