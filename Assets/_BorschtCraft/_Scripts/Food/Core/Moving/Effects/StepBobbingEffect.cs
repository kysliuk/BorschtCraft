using DG.Tweening;
using UnityEngine;

namespace BorschtCraft.Food
{
    public class StepBobbingEffect : IMoveEffect
    {
        private readonly float _amplitude;
        private readonly float _frequency;

        public StepBobbingEffect(float amplitude = 0.15f, float frequency = 5f)
        {
            _amplitude = amplitude;
            _frequency = frequency;
        }

        public void ApplyTo(Transform target, Vector3 start, Vector3 end, float duration, Sequence sequence)
        {
            float stepDuration = duration / (_frequency * 2f);
            sequence.Join(target.DOLocalMoveY(
                target.localPosition.y + _amplitude,
                stepDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops((int)(_frequency * 2), LoopType.Yoyo)
            );
        }
    }
}
