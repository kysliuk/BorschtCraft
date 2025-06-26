using BorschtCraft.Food;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public static class IngredientUtils
    {
        public static bool MatchItemToIngredients(IConsumed item, IReadOnlyCollection<IConsumed> ingredients)
        {
            var itemIngredients = ConvertItemToIngredients(item);

            return IngredientsMatch(itemIngredients, ingredients);
        }

        public static IReadOnlyCollection<IConsumed> ConvertItemToIngredients(IConsumed item)
        {
            var result = new List<IConsumed> { item };
            result.AddRange(item.Ingredients);

            return result;
        }

        private static bool IngredientsMatch(IReadOnlyCollection<IConsumed> a, IReadOnlyCollection<IConsumed> b)
        {
            if (a.Count != b.Count)
                return false;

            var aCounts = GetTypeCounts(a);
            var bCounts = GetTypeCounts(b);

            return aCounts.Count == bCounts.Count &&
                   aCounts.All(pair => bCounts.TryGetValue(pair.Key, out int bCount) && bCount == pair.Value);
        }

        private static Dictionary<Type, int> GetTypeCounts(IEnumerable<IConsumed> ingredients)
        {
            var dict = new Dictionary<Type, int>();
            foreach (var ing in ingredients)
            {
                var type = ing.GetType();
                if (dict.ContainsKey(type))
                    dict[type]++;
                else
                    dict[type] = 1;
            }
            return dict;
        }
    }
}
