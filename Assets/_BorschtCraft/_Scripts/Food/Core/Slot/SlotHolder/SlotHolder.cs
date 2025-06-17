namespace BorschtCraft.Food
{
    public static class SlotHolder
    {
        public static ISlot[] Slots;

        public static void Initialize(ISlot[] slots)
        {
            Slots = slots;
        }
    }
}
