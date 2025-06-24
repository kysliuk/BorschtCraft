using System;
using System.Collections.Generic;

namespace BorschtCraft.Food.FirstTable
{
    public class TableIngredientsList : TableIngredientListBase
    {
        protected override IDrink GetDrink()
        {
            return ConsumeAbstractFactory.CreateConsumed<Drink>(0, null);
        }

        protected override IConsumed GetFirstLayer()
        {
            return ConsumeAbstractFactory.CreateConsumed<BreadCooked>(0, null);
        }

        protected override IReadOnlyCollection<IConsumable> GetIngredientsProviders()
        {
            return new IConsumable[]
            {
                ConsumeAbstractFactory.CreateConsumable<SaloStack>(0),
                ConsumeAbstractFactory.CreateConsumable<GarlicStack>(0),
                ConsumeAbstractFactory.CreateConsumable<OnionStack>(0),
                ConsumeAbstractFactory.CreateConsumable<MustardStack>(0),
                ConsumeAbstractFactory.CreateConsumable<HorseradishStack>(0),
        };
        }


    }
}
