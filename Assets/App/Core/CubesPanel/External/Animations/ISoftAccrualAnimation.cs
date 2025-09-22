using UnityEngine;

namespace App.Core.Core.External.Animations
{
    public interface ISoftAccrualAnimation
    {
        void PlayAnimation(Vector3 globalPosition, Transform parent, long amount);
    }
}