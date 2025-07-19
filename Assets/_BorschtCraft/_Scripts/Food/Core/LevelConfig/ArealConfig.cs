using UnityEngine;

namespace BorschtCraft.Food
{
    [CreateAssetMenu(menuName = "Level Configs/Area Config", fileName = "New Area Config")]
    public class AreaConfig : ScriptableObject
    {
        public string AreaName = "New Area";
        public SceneField Scene;
        public GameObject SceneContextPrefab;
        public GameObject TablePrefab;
        public GameObject CustomerManagerPrefab;
    }
}
