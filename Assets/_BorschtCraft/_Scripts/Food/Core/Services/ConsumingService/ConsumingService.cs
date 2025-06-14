using BorschtCraft.Food.Signals;
using System.Collections.Generic;
using Zenject;
using BorschtCraft.Food.Core.Services.ConsumingService.Strategies;

namespace BorschtCraft.Food
{
    public class ConsumingService : IConsumingService, System.IDisposable, IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots;
        private readonly IItemSlot[] _releasingSlots;
        private readonly List<IConsumptionStrategy> _strategies;

        public ConsumingService(SignalBus signalBus,
                                [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots,
                                [Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots,
                                List<IConsumptionStrategy> strategies)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _releasingSlots = releasingSlots;
            _strategies = strategies;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received consumable interaction request signal for {signal.ConsumableSource.GetType().Name}.");
            var consumableSource = signal.ConsumableSource;

            if (consumableSource == null) return;

            IConsumed finalItem = null;
            IItemSlot finalSlot = null;

            foreach (var strategy in _strategies)
            {
                if (strategy.TryExecute(consumableSource, _cookingSlots, _releasingSlots, out finalItem, out finalSlot))
                {
                    Logger.LogInfo(this, $"Consumable interaction for {consumableSource.GetType().Name} handled by {strategy.GetType().Name}. Resulting item: {finalItem?.GetType().Name} in slot: {finalSlot?.GetGameObject()?.name ?? "N/A"}");
                    return;
                }
            }
            Logger.LogWarning(this, $"No strategy handled the consumable interaction for {consumableSource.GetType().Name}.");
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }
    }
}
