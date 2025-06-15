using UnityEngine;
using UniRx;
using System;

namespace BorschtCraft.Food.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConsumedView : MonoBehaviour
    {
        public Type Type => _consumedViewModel.ModelType;

        protected ConsumedViewModel _consumedViewModel;
        protected SpriteRenderer _spriteRenderer;

        public virtual void SetViewModel(ConsumedViewModel viewModel)
        {
            _consumedViewModel = viewModel;

            if (_consumedViewModel == null)
            {
                EnableVisibility(false);
                return;
            }

            _consumedViewModel.IsVisible.Subscribe(EnableVisibility).AddTo(this);
        }

        protected virtual void EnableVisibility(bool enable)
        {
            _spriteRenderer.enabled = enable;
        }

        #region Unity behaviour
        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        #endregion
    }
}
