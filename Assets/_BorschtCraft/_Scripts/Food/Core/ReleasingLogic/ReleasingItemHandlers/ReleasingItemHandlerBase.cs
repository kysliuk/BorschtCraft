namespace BorschtCraft.Food
{
    public abstract class ReleasingItemHandlerBase : ItemHandlerBase, IReleasingItemHandler
    {
        protected ISlotMatchingStrategy _strategy => SetStrategy();
        protected IConsumed _consumed;
        protected ISlot _slot;

        public virtual void SetSlot(ISlot slot)
        {
            _slot = slot;
        }

        protected override bool CanHandle(IItem item)
        {
            if (item is not IConsumed c)
                return false;

            _consumed = c;
            return true;
        }

        protected override bool Process(IItem item)
        {
            if(_strategy.Matches(_slot, item))
            {

            }

            return false;
        }

        protected ISlot GetAvailableReceiver()
        {

        }

        protected abstract ISlotMatchingStrategy SetStrategy();
    }
}
