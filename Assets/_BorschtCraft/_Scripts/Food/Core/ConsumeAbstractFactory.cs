using System;

namespace BorschtCraft.Food
{
    public static class ConsumeAbstractFactory
    {
        public static T CreateConsumed<T>(int price, IConsumed item) where T : IConsumed
        {
            return (T)Activator.CreateInstance(typeof(T), price, item);
        }

        public static T CreateConsumable<T>(int price) where T : Item
        {
            return (T)Activator.CreateInstance(typeof(T), price);
        }
    }
}
