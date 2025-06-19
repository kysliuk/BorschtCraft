using Codice.CM.Interfaces;
using Zenject;

namespace BorschtCraft.Food
{
    public abstract class TransferHandlerBase : ITransferHandler, IInitializable
    {
        protected ITransferableReceiver[] _slots;

        private ITransferHandler _nextHandler;
        private ITransferableReceiversRegistry _transferableRegistry;

        public bool Handle(IItem item)
        {
            if (CanHandle(item))
                return Process(item);

            return _nextHandler?.Handle(item) ?? false;
        }

        public void SetNext(ITransferHandler nextHandler) => _nextHandler = nextHandler;

        public void Initialize()
        {
            _slots = _transferableRegistry.Receivers;
            Logger.LogInfo(this, $"Number of slots: {_slots.Length}.");
        }

        protected abstract bool CanHandle(IItem item);
        protected abstract bool Process(IItem item);

        [Inject]
        public void Construct(ITransferableReceiversRegistry transferableRegistry)
        {
            _transferableRegistry = transferableRegistry;
            Logger.LogInfo(this, $"Constructed with {nameof(transferableRegistry)}, is null: {_transferableRegistry == null}");
        }

        
    }
}
