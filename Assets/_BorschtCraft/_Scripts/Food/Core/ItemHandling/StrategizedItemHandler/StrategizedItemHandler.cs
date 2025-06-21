using System;

namespace BorschtCraft.Food
{
    public abstract class StrategizedItemHandler<T> : ItemHandlerBase where T : ISlotMatchingStrategy
    {
        protected readonly T _strategy;
        protected StrategizedItemHandler()
        {
            _strategy =  Activator.CreateInstance<T>();
        }
    }
}
