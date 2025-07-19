using System.Collections.Generic;
using UnityEngine;

namespace BorschtCraft.Food
{
    [CreateAssetMenu(menuName = "Level Configs/Consumable Price List", fileName = "New Consumable Price List")]
    public class ConsumablePriceList : ScriptableObject
    {
        [SerializeField]
        public List<ConsumablePrice> PriceList = new List<ConsumablePrice>();
    }
}
