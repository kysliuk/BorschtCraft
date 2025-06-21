namespace BorschtCraft.Food
{
    public class HorseradishStack : Consumable<Horseradish>
    {
        protected override bool InnerCanDecorate(IConsumed item)
        {
            return item is IFifthLayer || item is IThirdLayer;
        }

        public HorseradishStack(int price) : base(price)
        {
        }
    }
}
