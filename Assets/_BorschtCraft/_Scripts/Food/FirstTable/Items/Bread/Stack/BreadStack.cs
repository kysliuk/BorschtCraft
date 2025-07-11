﻿namespace BorschtCraft.Food
{
    public class BreadStack : Consumable<BreadRaw>, ICantDecorate
    {
        protected override bool CanDecorate(IConsumed item = null)
        {
            return item == null;
        }

        public BreadStack(int price) : base(price)
        {
        }
    }
}
