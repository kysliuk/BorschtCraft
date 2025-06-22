namespace BorschtCraft.Food
{
    public class BreadCooked : Cooked<BreadCooked>
    {
        public override bool CanPlaceOnTop(IConsumed consumed, out IConsumed outConsumed)
        {
            if(consumed == null)
                return base.CanPlaceOnTop(consumed, out outConsumed);

            outConsumed = null;
            return false;
        }

        public BreadCooked(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
