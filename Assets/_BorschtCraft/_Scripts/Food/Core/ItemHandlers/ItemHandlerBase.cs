using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlerBase : IItemHandler
    {
        [Inject] public ISlot[] _slots { get; private set; }

        private IItemHandler _nextHandler;

        public bool Handle(IItem item)
        {
            if (CanHandle(item))
                return Process(item);

            return _nextHandler?.Handle(item) ?? false;
        }

        protected abstract bool CanHandle(IItem item);
        protected abstract bool Process(IItem item);

        protected void SetNext(IItemHandler nextHandler) => _nextHandler = nextHandler;
    }
}
