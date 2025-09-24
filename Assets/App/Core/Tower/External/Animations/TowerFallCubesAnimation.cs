using System.Collections.Generic;
using App.Core.Tower.External.Cube;
using DG.Tweening;
using UnityEngine;

namespace App.Core.Tower.External.Animations
{
    public class TowerFallCubesAnimation
    {
        public void PlayFallAnimation(int startIndex, IReadOnlyList<TowerCubePresenter> cubePresenters)
        {
            if (startIndex == cubePresenters.Count - 1)
            {
                return;
            }

            const float duration = 0.20f;
            for (int i = startIndex; i < cubePresenters.Count; ++i)
            {
                var cubePresenter = cubePresenters[i];
                var data = cubePresenter.TowerCube.Data;
                var rectTransform = cubePresenter.View.RectTransform;
                var previousPosition = new Vector3(data.PositionX, data.PositionY, 0);
                var newPosition = previousPosition + Vector3.down * (rectTransform.rect.height * rectTransform.lossyScale.y);
                rectTransform.DOMoveY(newPosition.y, duration);
                data.PositionX = newPosition.x;
                data.PositionY = newPosition.y;
            }
        }
    }
}