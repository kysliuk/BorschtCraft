using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlerBase : IInitializable
    {
        protected ISlot[] _slots;

        private IItemHandler _nextHandler;
        private ISlotRegistry _slotRegistry;

        public bool Handle(IItem item)
        {
            var processed = false;
            if (CanHandle(item))
                processed = Process(item);

            return processed ? processed : _nextHandler?.Handle(item) ?? false;
        }

        public void SetNext(IItemHandler nextHandler) => _nextHandler = nextHandler;

        public void Initialize()
        {
            _slots = _slotRegistry.Slots;
            Logger.LogInfo(this, $"Number of slots: {_slots.Length}.");
        }

        protected abstract bool CanHandle(IItem item);
        protected abstract bool Process(IItem item);

        [Inject]
        public void Construct(ISlotRegistry slotRegistry)
        {
            _slotRegistry = slotRegistry;
            Logger.LogInfo(this, $"Constructed with {nameof(slotRegistry)}, is null: {_slotRegistry == null}");
        }

        
    }
}
