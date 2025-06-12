namespace BorschtCraft.Food
{
    public class HorseradishStack : Consumable<Horseradish>
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

        public HorseradishStack(int price) : base(price)
        {
        }
    }
}
