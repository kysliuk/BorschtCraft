﻿namespace BorschtCraft.Food
{
    public class BreadRaw : Cookable<BreadCooked>
    {
        public BreadRaw(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
