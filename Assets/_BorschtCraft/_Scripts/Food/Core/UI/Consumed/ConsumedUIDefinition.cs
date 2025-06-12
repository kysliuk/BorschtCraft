using System;
using UnityEngine;

namespace BorschtCraft.Food.UI
{
    public class ConsumedUIDefinition
    {
        public Type ConsumedType { get; }
        public Type ViewModelType { get; }
        public Type ViewComponentType { get; }
        public GameObject ViewPrefab { get; }

        public ConsumedUIDefinition(Type consumedType, Type viewModelType, Type viewComponentType, GameObject viewPrefab)
        {
            ConsumedType = consumedType;
            ViewModelType = viewModelType;
            ViewComponentType = viewComponentType;
            ViewPrefab = viewPrefab;
        }
    }
}
