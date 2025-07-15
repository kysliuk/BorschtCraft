using DG.Tweening;
using UnityEngine;

namespace BorschtCraft.Food
{
    public interface IMoveEffect
    {
        void ApplyTo(Transform target, Vector3 start, Vector3 end, float duration, Sequence sequence);
    }
}
