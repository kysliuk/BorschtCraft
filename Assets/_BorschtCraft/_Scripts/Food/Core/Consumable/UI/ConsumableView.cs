using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public abstract class ConsumableView<T1, T2> : MonoBehaviour, IPointerClickHandler
        where T1 : Consumable<T2>
        where T2 : Consumed
    {
        protected ConsumableViewModel<T1, T2> _consumableViewModel;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Logger.LogInfo(this, $"Clicked on {_consumableViewModel?.GetType()?.Name}");
            _consumableViewModel?.AttemptConsume();
        }

        [Inject]
        public void Construct(ConsumableViewModel<T1, T2> viewModel)
        {
            _consumableViewModel = viewModel;
            Logger.LogInfo(this, $"Constructed with view model: {_consumableViewModel?.GetType()?.Name}<{typeof(T1).Name}, {typeof(T2).Name}>");
        }
    }
}
