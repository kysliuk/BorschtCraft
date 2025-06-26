using System;
using System.Threading.Tasks;
using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlerBase : IInitializable, IDisposable
    {
        protected ISlotRegistry _slotRegistry;
        protected SignalBus _signalBus;

        public void Initialize() => OnInitialize();

        public void Dispose() => OnDispose();


        protected abstract bool CanHandle(IItem item);

        protected abstract Task<bool> Process(IItem item);

        protected virtual void OnInitialize()
        {
            throw new NotImplementedException("OnInitialize method must be overridden in derived classes.");
        }

        protected virtual void OnDispose()
        {
            throw new NotImplementedException("OnDispose method must be overridden in derived classes.");
        }

        protected async void Handle(IItem item)
        {
            if (CanHandle(item))
                await Process(item);
        }

        [Inject]
        public void Construct(ISlotRegistry slotRegistry, SignalBus signalBus)
        {
            _slotRegistry = slotRegistry;
            _signalBus = signalBus;
        }
    }
}
