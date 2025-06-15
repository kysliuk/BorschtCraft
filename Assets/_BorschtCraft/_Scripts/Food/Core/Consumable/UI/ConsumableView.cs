using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public abstract class ConsumableView<T1, T2> : MonoBehaviour, IPointerClickHandler
        where T1 : Consumable<T2>
        where T2 : Consumed
    {
        protected ConsumableViewModel<T1, T2> _viewModel;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Logger.LogInfo(this, $"Clicked on {_viewModel?.GetType()?.Name}");
            _viewModel?.AttemptConsume();
        }

        [Inject]
        public void Construct(ConsumableViewModel<T1, T2> viewModel)
        {
            _viewModel = viewModel;
            Logger.LogInfo(this, $"Constructed with view model: {_viewModel?.GetType()?.Name}");
        }
    }
}
