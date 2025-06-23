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

        [Header("Fill Parameters")]
        [SerializeField] private float _fillDuration = 2.0f;
        [SerializeField] private float _emptyDuration = 1.0f;

        private Vector3 _initialMaskPosition;
        private Vector3 _initialMaskScale;
        private float _minFillHeight = 0.01f;
        private SignalBus _signalBus;

        void Start()
        {
            if (_liquidMaskTransform == null || _filledLiquidSpriteRenderer == null)
            {
                Logger.LogInfo(this, "Mask Transform or Filled Liquid SpriteRenderer not assigned!");
                enabled = false;
                return;
            }

            Bounds maskBounds = _liquidMaskTransform.GetComponent<SpriteMask>().sprite.bounds;

            _initialMaskScale = _liquidMaskTransform.localScale;
            _initialMaskPosition = _liquidMaskTransform.localPosition;
            _liquidMaskTransform.localScale = new Vector3(_initialMaskScale.x, _minFillHeight, _initialMaskScale.z);
            float yOffset = (_initialMaskScale.y - _minFillHeight) * maskBounds.extents.y;
            _liquidMaskTransform.localPosition = new Vector3(_initialMaskPosition.x, _initialMaskPosition.y - yOffset, _initialMaskPosition.z);
        }

        private async void OnFillGlassSignal(FillGlassSignal signal)
        {
            Logger.LogInfo(this, "Received signal for filling glass");
            StartFilling();

            await UniTask.Delay(TimeSpan.FromSeconds(_fillDuration));

            _signalBus.Fire<GlassFilledSignal>();
            StartEmptying();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<FillGlassSignal>(OnFillGlassSignal);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<FillGlassSignal>(OnFillGlassSignal);
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
            float elapsedTime = 0f;

            Vector3 startScale = new Vector3(_initialMaskScale.x, Mathf.Lerp(_minFillHeight, _initialMaskScale.y, startFillPercent), _initialMaskScale.z);
            Vector3 endScale = new Vector3(_initialMaskScale.x, Mathf.Lerp(_minFillHeight, _initialMaskScale.y, endFillPercent), _initialMaskScale.z);

            Bounds maskSpriteBounds = _liquidMaskTransform.GetComponent<SpriteMask>().sprite.bounds;
            float startYOffset = (_initialMaskScale.y - startScale.y) * (maskSpriteBounds.extents.y / _initialMaskScale.y); // Adjust based on mask's own scale
            float endYOffset = (_initialMaskScale.y - endScale.y) * (maskSpriteBounds.extents.y / _initialMaskScale.y);

            Vector3 startPosition = new Vector3(_initialMaskPosition.x, _initialMaskPosition.y - startYOffset, _initialMaskPosition.z);
            Vector3 endPosition = new Vector3(_initialMaskPosition.x, _initialMaskPosition.y - endYOffset, _initialMaskPosition.z);


            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);

                _liquidMaskTransform.localScale = Vector3.Lerp(startScale, endScale, progress);

                _liquidMaskTransform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);

                yield return null;
            }

            _liquidMaskTransform.localScale = endScale;
            _liquidMaskTransform.localPosition = endPosition;

            Logger.LogInfo(this, "Animation complete. Fill level: " + endFillPercent * 100 + "%");
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}