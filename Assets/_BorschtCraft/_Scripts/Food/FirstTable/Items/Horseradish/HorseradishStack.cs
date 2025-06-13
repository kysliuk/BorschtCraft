namespace BorschtCraft.Food
{
    public class HorseradishStack : Consumable<Horseradish>
    {
        public override bool InnerCanDecorate(IConsumed item)
        {
            return item is IFourthLayer || item is IThirdLayer;
        }

        public HorseradishStack(int price) : base(price)
        {
        }
    }
}
