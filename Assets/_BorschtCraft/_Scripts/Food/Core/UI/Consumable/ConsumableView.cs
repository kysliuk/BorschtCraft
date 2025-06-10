using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public abstract class ConsumableView<T1, T2, T3> : MonoBehaviour, IPointerClickHandler
        where T1 : ConsumableViewModel<T2, T3>
        where T2 : Consumable<T3>
        where T3 : Consumed
    {
        protected T1 _viewModel;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Logger.LogInfo(this, $"View model exists: {_viewModel != null}. {_viewModel?.GetType()?.Name}");
            _viewModel?.AttemptConsume();
        }

        [Inject]
        public void Construct(T1 viewModel)
        {
            _viewModel = viewModel;
            Logger.LogInfo(this, $"Constructed with view model: {_viewModel?.GetType()?.Name}");
        }
    }

}
