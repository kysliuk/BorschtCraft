using UnityEngine;

namespace BorschtCraft.Food
{
    [CreateAssetMenu(menuName = "Level Configs/Slot Config", fileName = "New Slot Config")]
    public class SlotConfig : ScriptableObject
    {
        public SlotType SlotType;

        [Min(0f)]
        public float CookingTime;
    }
}
