namespace BorschtCraft.Food
{
    public class SaloStack : Consumable<Salo>
    {
        public override IConsumed Consume(IConsumed item)
        {
            if (item == null)
            {
                Logger.LogError(this, "cannot consume null item. There should be an item.");
                return item;
            }

            return base.Consume(item);
        }

        public SaloStack(int price) : base(price)
        {
        }
    }
}
