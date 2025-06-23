using System;

namespace BorschtCraft.Food
{
    public class FillGlassSignal
    {
        public Type Type { get; }

        public FillGlassSignal(Type type)
        {
            Type = type;
        }
    }
}
