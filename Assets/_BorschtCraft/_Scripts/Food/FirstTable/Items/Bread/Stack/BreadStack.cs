namespace BorschtCraft.Food
{
    public class BreadStack : Consumable<BreadRaw>
    {
        public override IConsumed Consume(IConsumed item = null)
        {
            if (item != null)
            {
                Logger.LogError(this, $"BreadStack cannot consume {item.GetType().Name}. There shouldn't be a item.");
                return item;
            }

            return base.Consume(item);
        }

        public BreadStack(int price) : base(price)
        {
        }
    }
}
