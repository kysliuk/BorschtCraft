using System;

namespace BorschtCraft.Food
{
    public static class BreadFactory
    {
        public static T CreateConsumed<T>(int price, Item item) where T : Consumed
        {
            return (T)Activator.CreateInstance(typeof(T), price, item);
        }

        public static T CreateConsumable<T>(int price) where T : Item
        {
            return (T)Activator.CreateInstance(typeof(T), price);
        }
    }
}
