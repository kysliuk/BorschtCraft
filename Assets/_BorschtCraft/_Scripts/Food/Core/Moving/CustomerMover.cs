using System;
using UnityEngine;

namespace BorschtCraft.Food
{
    public class CustomerMover : Mover
    {
        [SerializeField] private float _horizontalOffsetRange = 2f;
        [SerializeField] private float _fixedYPosition = -3.5f;
        [SerializeField] private float _bobbingAmplitude = 0.15f;
        [SerializeField] private float _bobbingFrequency = 5f;

        private IMoveEffect _stepEffect;

        protected override void Awake()
        {
            base.Awake();
            _stepEffect = new StepBobbingEffect(_bobbingAmplitude, _bobbingFrequency);
        }

        public void MoveCustomerIn(Action onArrived = null)
        {
            Vector3 center = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _depth));

            Vector3 offScreenPos = _camera.ViewportToWorldPoint(new Vector3(-0.2f, 0.5f, _depth));
            transform.position = new Vector3(offScreenPos.x, _fixedYPosition, center.z);

            float targetX = center.x + UnityEngine.Random.Range(-_horizontalOffsetRange, _horizontalOffsetRange);
            Vector3 targetPos = new Vector3(targetX, _fixedYPosition, center.z);

            Logger.LogInfo(this, $"Moving customer in. Target position: {targetPos}. StepEffect: {_stepEffect.GetType().Name ?? "NONE"}");
            DoMove(onArrived, targetPos, true, _stepEffect);
        }

        public void MoveCustomerOut(Action onExited = null)
        {
            Vector3 currentPos = transform.position;
            Vector3 offScreenExit = _camera.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, _depth));
            Vector3 targetPos = new Vector3(offScreenExit.x, _fixedYPosition, currentPos.z);

            DoMove(onExited, targetPos, false, _stepEffect);
        }
    }
}
