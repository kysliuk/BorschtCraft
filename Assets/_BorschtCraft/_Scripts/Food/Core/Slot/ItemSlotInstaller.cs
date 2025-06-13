using UnityEngine;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotInstaller : MonoInstaller
    {
        [Tooltip("The role of this specific slot.")]
        [SerializeField] private SlotType _slotType = SlotType.Undefined;

        public override void InstallBindings()
        {
            Container.Bind<IItemSlot>()
                     .To<ItemSlot>()
                     .FromInstance(new ItemSlot(_slotType))
                     .AsSingle().NonLazy();
        }
    }
}