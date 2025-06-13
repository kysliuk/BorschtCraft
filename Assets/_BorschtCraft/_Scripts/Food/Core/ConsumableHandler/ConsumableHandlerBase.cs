namespace BorschtCraft.Food.Handlers
{
    public abstract class ConsumableHandlerBase : IConsumableHandler
    {
        private IConsumableHandler _next;

        public void SetNext(IConsumableHandler next)
        {
            _next = next;
        }

        public bool Handle(IConsumable consumable)
        {
            if (CanHandle(consumable))
            {
                return Process(consumable);
            }
            return _next?.Handle(consumable) ?? false;
        }

        protected abstract bool CanHandle(IConsumable consumable);
        protected abstract bool Process(IConsumable consumable);
    }
}