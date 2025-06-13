namespace BorschtCraft.Food
{
    public class SaloStack : Consumable<Salo>
    {
        public override bool InnerCanDecorate(IConsumed item)
        {
            return item is ICooked || item is ISecondLayer;
        }
        public SaloStack(int price) : base(price)
        {
        }
    }
}
