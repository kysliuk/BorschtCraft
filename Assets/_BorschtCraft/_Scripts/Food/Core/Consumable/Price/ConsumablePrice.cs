using System;

namespace BorschtCraft.Food
{
    [Serializable]
    public class ConsumablePrice
    {
        public string Type;
        public int Price;

        public ConsumablePrice(string type, int price)
        {
            Type = type;
            Price = price;
        }
    }
}
