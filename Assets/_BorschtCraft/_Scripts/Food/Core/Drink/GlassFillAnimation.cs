using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

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
    private float _maxFillHeight;
    private float _minFillHeight = 0.01f;

    void Start()
    {
        if (_liquidMaskTransform == null || _filledLiquidSpriteRenderer == null)
        {
            Debug.LogError("Mask Transform or Filled Liquid SpriteRenderer not assigned!");
            enabled = false;
            return;
        }

        Bounds maskBounds = _liquidMaskTransform.GetComponent<SpriteMask>().sprite.bounds;

        _maxFillHeight = _liquidMaskTransform.localScale.y;

        _initialMaskScale = _liquidMaskTransform.localScale;
        _initialMaskPosition = _liquidMaskTransform.localPosition;
        _liquidMaskTransform.localScale = new Vector3(_initialMaskScale.x, _minFillHeight, _initialMaskScale.z);
        float yOffset = (_initialMaskScale.y - _minFillHeight) * maskBounds.extents.y;
        _liquidMaskTransform.localPosition = new Vector3(_initialMaskPosition.x, _initialMaskPosition.y - yOffset, _initialMaskPosition.z);
    }

    public void StartFilling()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateFill(0f, 1f, _fillDuration));
    }

    public void StartEmptying()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateFill(1f, 0f, _emptyDuration));
    }


    IEnumerator AnimateFill(float startFillPercent, float endFillPercent, float duration)
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

        Debug.Log("Animation complete. Fill level: " + endFillPercent * 100 + "%");
    }


    [ContextMenu(nameof(FillAndEmptyRoutine))]
    public async void FillAndEmptyRoutine()
    {
        StartEmptying();
        await UniTask.Delay(1000);
        StartFilling();
    }

}