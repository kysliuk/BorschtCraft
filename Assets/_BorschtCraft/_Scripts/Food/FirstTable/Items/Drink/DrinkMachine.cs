namespace BorschtCraft.Food
{
    public class DrinkMachine : Consumable<Drink>
    {
        protected override bool CanDecorate(IConsumed item)
        {
            return item == null;
        }

        public DrinkMachine(int price) : base(price)
        {
        }
    }
}
