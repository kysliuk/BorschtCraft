using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class SlotView : MonoBehaviour, IPointerClickHandler
    {
        public SlotViewModel SlotViewModel => _slotViewModel;

        protected SlotViewModel _slotViewModel;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (_slotViewModel == null)
            {
                Logger.LogWarning(this, "SlotViewModel is not set. Cannot handle pointer click.");
                return;
            }

            Logger.LogInfo(this, $"Clicked on slot with item: {_slotViewModel.CurrentItem.Value?.GetType().Name ?? "None"}");
            _slotViewModel.ReleaseItem();
        }

        [Inject]
        public void Construct(SlotViewModel slotViewModel)
        {
            _slotViewModel = slotViewModel;
        }
    }
}
