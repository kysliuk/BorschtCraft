using System;

namespace BorschtCraft.Food
{
    public class GlassFilledSignal
    {
        public Guid FillingId { get; }

        public GlassFilledSignal(Guid fillingId)
        {
            FillingId = fillingId;
        }
    }
}
