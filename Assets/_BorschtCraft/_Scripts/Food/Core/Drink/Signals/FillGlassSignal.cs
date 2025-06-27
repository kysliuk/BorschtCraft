using System;

namespace BorschtCraft.Food
{
    public class FillGlassSignal
    {
        public Guid FillingId { get; }

        public FillGlassSignal()
        {
            FillingId = Guid.NewGuid();
        }
    }
}
