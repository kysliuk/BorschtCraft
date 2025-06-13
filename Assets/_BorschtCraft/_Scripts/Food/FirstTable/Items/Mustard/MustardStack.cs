namespace BorschtCraft.Food
{
    public class MustardStack : Consumable<Mustard>
        {
        protected override bool InnerCanDecorate(IConsumed item)
        {
            return item is IFourthLayer || item is IThirdLayer;
        }
        public MustardStack(int price) : base(price)
        {
        }
    }
}
