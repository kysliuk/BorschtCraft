using BorschtCraft.Food.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace BorschtCraft.Food.FirstTable
{
    public class ConsumableInstaller : InstallerBase
    {
        private List<ConsumablePrice> _foodPrices;

        public override void Install()
        {
            InstallConsumables();
            InstallSignals();
        }

        private void InstallConsumables()
        {
            InstallConsumable<BreadStack, BreadRaw>();
            InstallConsumable<SaloStack, Salo>();
            InstallConsumable<GarlicStack, Garlic>();
            InstallConsumable<HorseradishStack, Horseradish>();
            InstallConsumable<MustardStack, Mustard>();
            InstallConsumable<OnionStack, Onion>();
            InstallConsumable<DrinkMachine, Drink>();
        }

        private void InstallSignals()
        {
            _container.DeclareSignal<ConsumableInteractionRequestSignal>();
        }

        private void InstallConsumable<T1, T2>() where T1 : IConsumable where T2 : IConsumed
        {
            var price = _foodPrices.Find(f => f.Type == typeof(T1).Name)?.Price ?? throw new Exception($"No price for {typeof(T1).Name}");
            new GenericConsumableInstaller<T1, T2>().Install(_container, price);
            Logger.Log($"Installed consumable: {typeof(T1).Name} with price: {price}");
        }

        public ConsumableInstaller(DiContainer container, List<ConsumablePrice> foodPrices) : base(container)
        {
            _foodPrices = foodPrices;
        }
    }
}
