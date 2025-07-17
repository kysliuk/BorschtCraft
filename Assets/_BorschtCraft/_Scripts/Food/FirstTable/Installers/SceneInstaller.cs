using Zenject;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BorschtCraft.Food.FirstTable
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private List<FoodPrice> _foodPrices =
            new List<FoodPrice>
            { new FoodPrice(nameof(BreadRaw), 10),
              new FoodPrice(nameof(Salo), 15),
              new FoodPrice(nameof(Garlic), 5),
              new FoodPrice(nameof(Horseradish), 7),
              new FoodPrice(nameof(Mustard), 8),
              new FoodPrice(nameof(Onion), 6),
              new FoodPrice(nameof(Drink), 12),
            };

        public override void InstallBindings()
        {
            //Bind slot registry
            Container.Bind<ISlotRegistry>().To<SlotRegistry>().AsSingle();
            Container.Bind<ISlotViewRegistry>().To<SlotViewRegistry>().AsSingle();

            //Install Slot Signals
            new SlotSignalsInstaller(Container).Install();

            //Install Consumables
            new ConsumableInstaller(Container, _foodPrices).Install();

            //Install Consumed
            new ConsumedInstaller(Container).Install();

            //Install Consuming Logic
            new ConsumingLogicInstaller(Container).Install();

            //Install Releasing Logic
            new ReleasingLogicInstaller(Container).Install();

            //Install Cooking Logic
            new CookingInstaller(Container).Install();

            //Install Drink signals
            new DrinkInstaller(Container).Install();
        }
    }
}
