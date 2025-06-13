namespace BorschtCraft.Food.Handlers
{
    public class DecorationHandler : ConsumableHandlerBase
    {
        private readonly ICombiningService _combiningService;

        public DecorationHandler(ICombiningService combiningService)
        {
            _combiningService = combiningService;
        }

        protected override bool CanHandle(IConsumable consumable)
        {
            return consumable is not ICantDecorate;
        }

        protected override bool Process(IConsumable consumable)
        {
            Logger.LogInfo(this, $"{consumable.GetType().Name} might be a decorator. Attempting combination.");
            return _combiningService.AttemptCombination(consumable);
        }
    }
}