using UnityEngine;

namespace BorschtCraft.Food
{
    [CreateAssetMenu(menuName = "Level Configs/Customer Config", fileName = "New Customer Config")]
    public class CustomerConfig : ScriptableObject
    {
        [Min(1)]
        public int MaxCustomers = 4;
        [Min(1f)]
        public float SpawnDelay = 3f;
        [Min(1f)]
        public float MaxWaitTime = 15f;
        [Min(1f)]
        public float MinDistanceBetweenCustomers = 1.5f;
    }
}
