using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
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
        }

        public void Dispose()
        {
        }
    }
}