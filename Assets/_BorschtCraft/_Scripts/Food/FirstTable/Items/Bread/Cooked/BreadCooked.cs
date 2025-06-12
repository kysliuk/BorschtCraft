using System.Diagnostics;

namespace BorschtCraft.Food
{
    public class BreadCooked : Consumed, ICooked
    {
        public BreadCooked(int price, Item item) : base(price, item)
        {
        }
    }
}
