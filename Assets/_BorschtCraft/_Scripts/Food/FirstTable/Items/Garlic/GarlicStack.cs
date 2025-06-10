namespace BorschtCraft.Food
{
    public class GarlicStack : Consumable<Garlic>
    {
        public override Garlic Consume(Consumed item)
        {
            if (item.GetType() != typeof(BreadCooked))
            {
                Logger.LogError(this, $"cannot be consumed for {item.GetType().Name}. Only {nameof(BreadCooked)} is allowed.");
                return null;
            }

            return base.Consume(item);
        }
        public GarlicStack(int price) : base(price)
        {
        }
    }
}
