using DG.Tweening;
using System;
using UnityEngine;

namespace BorschtCraft.Food
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] protected Ease _moveEase = Ease.Linear;
        [SerializeField] protected float _moveDuration = 1.5f;

        protected Camera _camera;
        protected float _depth;

        protected virtual void Awake()
        {
            _camera = Camera.main;
            _depth = Mathf.Abs(transform.position.z - _camera.transform.position.z);
        }

        public void DoMove(
            Action callback,
            Vector3 endValue,
            bool setActiveAfter = true,
            IMoveEffect effect = null)
        {
            Vector3 startPosition = transform.position;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(endValue, _moveDuration).SetEase(_moveEase));
            effect?.ApplyTo(transform, startPosition, endValue, _moveDuration, sequence);

            sequence.OnComplete(() =>
            {
                gameObject.SetActive(setActiveAfter);
                callback?.Invoke();
            });
        }
    }
}