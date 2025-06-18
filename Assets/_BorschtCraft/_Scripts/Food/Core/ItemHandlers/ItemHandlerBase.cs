using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlerBase : IItemHandler
    {
        protected ISlot[] _slots { get; private set; }

        private IItemHandler _nextHandler;

        public bool Handle(IItem item)
        {
            if(CanHandle(item))
                return Process(item);

            return _nextHandler?.Handle(item) ?? false;
        }

        public void SetNext(IItemHandler nextHandler) => _nextHandler = nextHandler;

        protected abstract bool CanHandle(IItem item);
        protected abstract bool Process(IItem item);

        [Inject]
        public void Construct(ISlot[] slots)
        {
            _slots = slots;
        }
    }
}
