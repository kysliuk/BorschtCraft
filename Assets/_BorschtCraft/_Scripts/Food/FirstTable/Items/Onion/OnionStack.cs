namespace BorschtCraft.Food
{
    public class OnionStack : Consumable<Onion>
    {
        public override Onion Consume(Consumed item)
        {
            if (item.GetType() != typeof(BreadCooked))
            {
                Logger.LogError(this, $"cannot be consumed for {item.GetType().Name}. Only {nameof(BreadCooked)} is allowed.");
                return null;
            }

            return base.Consume(item);
        }
        public OnionStack(int price) : base(price)
        {
        }
    }
}
