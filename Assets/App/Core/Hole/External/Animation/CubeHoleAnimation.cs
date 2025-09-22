using App.Core.Core.External;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using DG.Tweening;
using UnityEngine;

namespace App.Core.Hole.External.Animation
{
    public class CubeHoleAnimation
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly HoleView m_View;

        public CubeHoleAnimation(ICoreUIController coreUIController, HoleView view)
        {
            m_CoreUIController = coreUIController;
            m_View = view;
        }

        public bool DropInHole(CubeView view, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_View.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("Cant create cube view");
                return false;
            }

            cubeView.Value.SetGlobalPosition(view.GetGlobalPosition());
            var transform = cubeView.Value.transform;
            DOTween.Sequence()
                .Append(transform.DOMove(m_View.Top.position, 0.5f).SetEase(Ease.OutQuad))
                .Join(transform.DORotate(new Vector3(0, 0, 45), 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() => transform.SetParent(m_View.MaskTransform))
                .Append(transform.DOMove(m_View.Bottom.position, 0.5f).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    m_CoreUIController.DestroyCubeView(cubeView.Value);
                });

            return true;
        }
    }
}