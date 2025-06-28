using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using System.Collections;

namespace BorschtCraft.Food.UI
{
    public class SlotView : MonoBehaviour, IPointerClickHandler
    {
        public SlotViewModel SlotViewModel => _slotViewModel;
        protected SlotViewModel _slotViewModel;

        private float _doubleClickThreshold = 0.15f;
        private Coroutine _clickCoroutine;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Logger.LogInfo(this, $"Click count: {eventData.clickCount}");

            if (_slotViewModel == null)
            {
                Logger.LogWarning(this, "SlotViewModel is not set. Cannot handle pointer click.");
                return;
            }

            if (_clickCoroutine != null)
            {
                StopCoroutine(_clickCoroutine);
                _clickCoroutine = null;
            }

            _clickCoroutine = StartCoroutine(HandleClick());
        }

        private IEnumerator HandleClick()
        {
            float timer = 0f;
            bool doubleClickDetected = false;

            while (timer < _doubleClickThreshold)
            {
                timer += Time.deltaTime;
                if (Input.GetMouseButtonDown(0))
                {
                    doubleClickDetected = true;
                    break;
                }

                yield return null;
            }

            if (doubleClickDetected)
                HandleDoubleClick();
            else
                HandleSingleClick();

            _clickCoroutine = null;
        }

        private void HandleSingleClick()
        {
            Logger.LogInfo(this, $"Clicked on Slot {_slotViewModel.Slot.SlotType} with item: {_slotViewModel?.CurrentItem?.Value?.GetType().Name ?? "None"}");
            _slotViewModel.ReleaseItem();
        }

        private void HandleDoubleClick()
        {
            Logger.LogInfo(this, $"About to put item {_slotViewModel.CurrentItem.GetType().Name} to trash can");
            _slotViewModel.PutItemInTrashCan();
        }

        [Inject]
        public void Construct(SlotViewModel slotViewModel)
        {
            _slotViewModel = slotViewModel;
        }
    }
}
