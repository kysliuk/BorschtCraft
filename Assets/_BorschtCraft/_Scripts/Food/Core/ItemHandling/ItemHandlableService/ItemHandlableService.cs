using Codice.CM.Interfaces;
using Zenject;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlableService : IItemHandlableService
    {
        protected readonly SignalBus _signalBus;
        protected IItemHandler _itemHandler;

        public ItemHandlableService(SignalBus signalBus, IItemHandler itemHandler)
        {
            _signalBus = signalBus;
            _itemHandler = itemHandler;
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void Initialize()
        {
            OnInitialize();
        }

        protected abstract void OnInitialize();

        protected abstract void OnDispose();
    }
}
