using System;

namespace BorschtCraft.Food
{
    [Serializable]
    public class FoodPrice
    {
        public string Type;
        public int Price;

        public FoodPrice(string type, int price)
        {
            Type = type;
            Price = price;
        }
    }
}
