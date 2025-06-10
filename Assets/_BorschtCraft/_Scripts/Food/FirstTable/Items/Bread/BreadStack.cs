namespace BorschtCraft.Food
{
    public class BreadStack : Consumable<BreadRaw>
    {
        public override BreadRaw Consume(Consumed item = null)
        {
            if (item != null)
            {
                Logger.LogError(this, $"BreadStack cannot consume {item.GetType().Name}. There shouldn't be a item.");
                return null;
            }

            return base.Consume(item);
        }
        public BreadStack(int price) : base(price)
        {
        }
    }
}
