using UnityEngine;

namespace BorschtCraft.Food
{
    [CreateAssetMenu(menuName = "Level Configs/Customer Config", fileName = "New Customer Config")]
    public class CustomerConfig : ScriptableObject
    {
        public int MaxCustomers = 4;
        public float SpawnDelay = 3f;
        public float MaxWaitTime = 15f;
    }
}
