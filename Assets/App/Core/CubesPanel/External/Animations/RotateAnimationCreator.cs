using DG.Tweening;
using UnityEngine;

namespace App.Core.CubesPanel.External.Animations
{
    public class RotateAnimationCreator
    {
        public const float DefaultRotateTime = 7.77f;
        public const float LoadingRotateTime = 1.0f;
        
        public Sequence Create(Transform target, float rotateTime = DefaultRotateTime, bool loop = true, bool flip = false)
        {
            var endValue = new Vector3(0.0f, 0.0f, (flip ? -1 : 1) * 360.0f);
            var sequence = DOTween.Sequence();
            target.rotation = Quaternion.identity;
            sequence.Append(target.DORotate(
                endValue,
                rotateTime,
                RotateMode.FastBeyond360).SetEase(Ease.Linear));
            
            if (loop)
            {
                sequence.SetLoops(-1, LoopType.Restart);
            }

            return sequence;
        }
    }
}