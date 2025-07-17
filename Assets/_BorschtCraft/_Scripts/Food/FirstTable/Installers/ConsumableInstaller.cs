using BorschtCraft.Food.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace BorschtCraft.Food.FirstTable
{
    public class ConsumableInstaller : InstallerBase
    {
        private List<FoodPrice> _foodPrices;

        public override void Install()
        {
            InstallConsumables();
            InstallSignals();
        }

        private void InstallConsumables()
        {
            //new GenericConsumableInstaller<BreadStack, BreadRaw>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<SaloStack, Salo>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<GarlicStack, Garlic>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<HorseradishStack, Horseradish>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<MustardStack, Mustard>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<OnionStack, Onion>().Install(_container, _initialPrice);
            //new GenericConsumableInstaller<DrinkMachine, Drink>().Install(_container, _initialPrice);

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
            var price = _foodPrices.Find(f => f.Type == typeof(T1).Name)?.Price ?? 0;
            new GenericConsumableInstaller<T1, T2>().Install(_container, price);
        }

        public ConsumableInstaller(DiContainer container, List<FoodPrice> foodPrices) : base(container)
        {
            _foodPrices = foodPrices;
        }
    }
}
