using System;

namespace BorschtCraft.Food
{
    public abstract class StrategizedItemHandler<T> : ItemHandlerBase where T : ISlotMatchingStrategy
    {
        protected ISlotMatchingStrategy _strategy => SetStrategy();
        private ISlotMatchingStrategy SetStrategy()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
