using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class SlotView : MonoBehaviour, IPointerClickHandler
    {
        protected SlotViewModel _slotViewModel;
        //protected

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

        private void OnItemChanged(IConsumed item)
        {
            
        }

        [Inject]
        public void Construct(SlotViewModel slotViewModel)
        {
            _slotViewModel = slotViewModel;
            _slotViewModel.CurrentItem.Subscribe(OnItemChanged).AddTo(this);

        }
    }
}
