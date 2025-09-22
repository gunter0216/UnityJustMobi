using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using DG.Tweening;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class HoleController : IInitSystem, IHoleController, IDisposable 
    {
        private readonly ICoreUIController m_CoreUIController;
        
        private HoleView m_View;

        public HoleController(ICoreUIController coreUIController)
        {
            m_CoreUIController = coreUIController;
        }

        public void Init()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }

            m_View = view.Value.HoleView;
        }

        public void DropInHole(CubeView view, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_View.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("Cant create cube view");
                return;
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
        }

        public void Dispose()
        {
        }
    }
}