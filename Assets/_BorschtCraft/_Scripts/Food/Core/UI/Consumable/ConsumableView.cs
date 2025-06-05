using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumableView<T1, T2> : MonoBehaviour, IPointerClickHandler where T1 : Consumable<T2> where T2 : Consumed 
    {
        private ConsumableViewModel<T1, T2> _viewModel;

        public void OnPointerClick(PointerEventData eventData)
        {
            _viewModel.AttemptConsume();
        }

        [Inject]
        public void Construct(ConsumableViewModel<T1, T2> viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
