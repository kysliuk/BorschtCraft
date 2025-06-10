namespace BorschtCraft.Food
{
    public class MustardStack : Consumable<Mustard>
    {
        public override Mustard Consume(Consumed item)
        {
            if (item == null)
            {
                Logger.LogError(this, "cannot consume null item. There should be an item.");
                return null;
            }

            return base.Consume(item);
        }

        public MustardStack(int price) : base(price)
        {
        }
    }
}
