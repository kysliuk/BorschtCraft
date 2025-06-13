namespace BorschtCraft.Food.Signals
{
    public class SlotClickedSignal
    {
        public IItemSlot ClickedSlot { get; }

        public SlotClickedSignal(IItemSlot clickedSlot)
        {
            ClickedSlot = clickedSlot;
        }
    }
}