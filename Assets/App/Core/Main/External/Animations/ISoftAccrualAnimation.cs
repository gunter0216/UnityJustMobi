using UnityEngine;

namespace App.Core.Main.External.Animations
{
    public interface ISoftAccrualAnimation
    {
        void PlayAnimation(Vector3 globalPosition, Transform parent, long amount);
    }
}