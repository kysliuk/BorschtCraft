using UnityEngine;
using DG.Tweening;
using System;

namespace BorschtCraft.Food
{
    public class CustomerMover : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 1.5f;
        [SerializeField] private float _horizontalOffsetRange = 2f;
        [SerializeField] private float _fixedYPosition = -3.5f;
        [SerializeField] private Ease _moveEase = Ease.OutCubic;

        private Camera _camera;
        private float _depth;

        private void Awake()
        {
            _camera = Camera.main;
            _depth = Mathf.Abs(transform.position.z - _camera.transform.position.z);
        }

        public void MoveCustomerIn(Action onArrived = null)
        {
            Vector3 center = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _depth));
            float z = center.z;
            float y = _fixedYPosition;

            Vector3 offScreenPos = _camera.ViewportToWorldPoint(new Vector3(-0.2f, 0.5f, _depth));
            transform.position = new Vector3(offScreenPos.x, y, z);

            float targetX = center.x + UnityEngine.Random.Range(-_horizontalOffsetRange, _horizontalOffsetRange);
            Vector3 targetPos = new Vector3(targetX, y, z);

            DoMove(onArrived, targetPos);
        }

        public void MoveCustomerOut(Action onExited = null)
        {
            Vector3 currentPos = transform.position;
            Vector3 offScreenExit = _camera.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, _depth));
            Vector3 targetPos = new Vector3(offScreenExit.x, _fixedYPosition, currentPos.z);

            DoMove(onExited, targetPos, false);
        }

        private void DoMove(Action callback, Vector3 endValue, bool setActiveAfter = true)
        {
            transform.DOMove(endValue, _moveDuration)
                .SetEase(_moveEase)
                .OnComplete(() =>
                {
                    gameObject.SetActive(setActiveAfter);
                    callback?.Invoke();
                });
        }
    }
}
