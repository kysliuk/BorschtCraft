using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Cysharp.Threading.Tasks;

namespace BorschtCraft.Food
{
    public class GlassFillAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _liquidMaskTransform;
        [SerializeField] private SpriteRenderer _filledLiquidSpriteRenderer;

        [Header("Configuration")]
        [SerializeField] private float _fillDuration = 2.0f;
        [SerializeField] private float _emptyDuration = 1.0f;
        [SerializeField] private float _serveDelay = 0.5f;

        private Vector3 _initialMaskPosition;
        private Vector3 _initialMaskScale;
        private float _minFillHeight = 0.00f;
        private Bounds _maskSpriteBounds;
        private SignalBus _signalBus;
        private bool _isFilling = false;

        void Start()
        {
            if (_liquidMaskTransform == null || _filledLiquidSpriteRenderer == null)
            {
                Logger.LogInfo(this, "Mask Transform or Filled Liquid SpriteRenderer not assigned!");
                enabled = false;
                return;
            }

            _maskSpriteBounds = _liquidMaskTransform.GetComponent<SpriteMask>().sprite.bounds;

            _initialMaskScale = _liquidMaskTransform.localScale;
            _initialMaskPosition = _liquidMaskTransform.localPosition;
            _liquidMaskTransform.localScale = new Vector3(_initialMaskScale.x, _minFillHeight, _initialMaskScale.z);
            float yOffset = (_initialMaskScale.y - _minFillHeight) * _maskSpriteBounds.extents.y;
            _liquidMaskTransform.localPosition = new Vector3(_initialMaskPosition.x, _initialMaskPosition.y - yOffset, _initialMaskPosition.z);
        }

        private async void OnFillGlassSignal(FillGlassSignal signal)
        {
            if (_isFilling)
                return;

            Logger.LogInfo(this, "Received signal for filling glass");
            StartFilling();

            await UniTask.Delay(TimeSpan.FromSeconds(_fillDuration + _serveDelay));

            _signalBus.Fire<GlassFilledSignal>();
            StartEmptying();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<FillGlassSignal>(OnFillGlassSignal);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<FillGlassSignal>(OnFillGlassSignal);
        }

        private void StartFilling()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateFill(0f, 1f, _fillDuration));
        }

        private void StartEmptying()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateFill(1f, 0f, _emptyDuration));
        }

        private IEnumerator AnimateFill(float startFillPercent, float endFillPercent, float duration)
        {
            _isFilling = true;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);
                float currentFillPercent = Mathf.Lerp(startFillPercent, endFillPercent, progress);

                SetMaskState(currentFillPercent);

                yield return null;
            }

            SetMaskState(endFillPercent);

            _isFilling = false;
            Logger.LogInfo(this, "Animation complete. Fill level: " + endFillPercent * 100 + "%");
        }

        private void SetMaskState(float fillPercent)
        {
            if (_liquidMaskTransform == null || _maskSpriteBounds == null) return;

            float targetYScale = Mathf.Lerp(_minFillHeight, _initialMaskScale.y, fillPercent);

            _liquidMaskTransform.localScale = new Vector3(_initialMaskScale.x, targetYScale, _initialMaskScale.z);

            float yOffset = _maskSpriteBounds.extents.y * (_initialMaskScale.y - targetYScale);

            _liquidMaskTransform.localPosition = new Vector3(
                _initialMaskPosition.x,
                _initialMaskPosition.y - yOffset,
                _initialMaskPosition.z
            );
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}