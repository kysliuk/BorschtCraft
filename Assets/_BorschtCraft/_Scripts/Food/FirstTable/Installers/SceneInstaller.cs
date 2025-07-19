using Zenject;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BorschtCraft.Food.FirstTable
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] ConsumablePriceList _foodPrices;
        public override void InstallBindings()
        {
            //Bind slot registry
            Container.Bind<ISlotRegistry>().To<SlotRegistry>().AsSingle();
            Container.Bind<ISlotViewRegistry>().To<SlotViewRegistry>().AsSingle();

            //Install Slot Signals
            new SlotSignalsInstaller(Container).Install();

            //Install Consumables
            new ConsumableInstaller(Container, _foodPrices.PriceList).Install();

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
