using App.Core.Core.External;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using DG.Tweening;
using UnityEngine;

namespace App.Core.CubeDragger.External.Animation
{
    public class CubeDisappearAnimation
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly CoreView m_CoreView;

        public CubeDisappearAnimation(ICoreUIController coreUIController, CoreView coreView)
        {
            m_CoreUIController = coreUIController;
            m_CoreView = coreView;
        }

        public bool Disappear(CubeView view, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_CoreView.DisappearCubeParent, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("Cant create cube view");
                return false;
            }

            cubeView.Value.SetGlobalPosition(view.GetGlobalPosition());
            var transform = cubeView.Value.transform;
            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    m_CoreUIController.DestroyCubeView(cubeView.Value);
                });

            return true;
        }
    }
}