using App.Core.CoreUI.External.View;
using App.Core.Tower.External.Cube;
using DG.Tweening;
using UnityEngine;

namespace App.Core.Tower.External.Animations
{
    public class JumpCubeAnimation
    {
        public void PlayJumpAnimation(CubeView dragView, TowerCubePresenter presenter, Vector3 newPosition)
        {
            const float duration = 0.20f;
            var dragCubePosition = dragView.RectTransform.position;
            presenter.View.SetGlobalPosition(dragCubePosition);
            presenter.View.RectTransform.DOJump(newPosition, 1, 1, duration);
        }
    }
}

