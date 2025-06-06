namespace BorschtCraft.Food
{
    public class BreadStack : Consumable<BreadRaw>
    {
        public override BreadRaw Consume(Consumed item = null)
        {
            return base.Consume(item);
        }
        public BreadStack(int price) : base(price)
        {
        }
    }
}
