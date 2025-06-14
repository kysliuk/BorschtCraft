using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ItemSlotView : MonoBehaviour, IPointerClickHandler
    {
        ItemSlotViewModel _itemSlotViewModel;
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            // Handle pointer click events here if needed
            Debug.Log("Item slot clicked.");
        }

        [Inject]
        public void Construct(ItemSlotViewModel itemSlotViewModel)
        {
            _itemSlotViewModel = itemSlotViewModel;
        }
    }
}
