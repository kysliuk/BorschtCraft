namespace BorschtCraft.Food
{
    public class HorseradishStack : Consumable<Horseradish>
    {
        public override Horseradish Consume(Consumed item)
        {
            if (item == null)
            {
                Logger.LogError(this, "cannot consume null item. There should be an item.");
                return null;
            }

            return base.Consume(item);
        }

        public HorseradishStack(int price) : base(price)
        {
        }
    }
}
