namespace BorschtCraft.Food
{
    public static class BreadFactory
    {
        public static T CreateConsumed<T>(int price, Item item) where T : Consumed
        {
            return new Consumed(price, item) as T;
        }

        public static T CreateConsumable<T>(int price) where T : Consumable
        {
            return new Consumable(price) as T;
        }
    }
}
