namespace BorschtCraft.Food
{
    public class BreadStack : Consumable<BreadRaw>, ICantDecorate
    {
        public override bool CanDecorate(IConsumed item = null)
        {
            if (item == null) return true;
            return false;
        }


        public BreadStack(int price) : base(price)
        {
        }
    }
}
