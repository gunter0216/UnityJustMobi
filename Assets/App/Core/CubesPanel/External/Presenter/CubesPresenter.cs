using System;
using System.Collections.Generic;
using App.Common.SpriteLoaders.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External;
using UnityEngine;

namespace App.Core.CubesPanel.External.Presenter
{
    public class CubesPresenter
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICoreUIController m_CoreUIController;
        private readonly CoreCubesView m_View;
        private readonly ICubesController m_CubesController;
        private readonly Action<TemplateCubePresenter> m_OnStartDrag;

        private List<TemplateCubePresenter> m_Cubes;

        public CubesPresenter(
            ICoreUIController coreUIController, 
            CoreCubesView view,
            ICubesController cubesController,
            ISpriteLoader spriteLoader,
            Action<TemplateCubePresenter> onStartDrag)
        {
            m_CoreUIController = coreUIController;
            m_View = view;
            m_CubesController = cubesController;
            m_SpriteLoader = spriteLoader;
            m_OnStartDrag = onStartDrag;
        }

        public void Initialize()
        {
            var cubes = m_CubesController.GetCubes();
            m_Cubes = new List<TemplateCubePresenter>(cubes.Count);
            foreach (var cubeConfig in cubes)
            {
                var view = m_CoreUIController.CreateCubeView(m_View.CubesContent, cubeConfig);
                if (!view.HasValue)
                {
                    Debug.LogError("Cant create cube view");
                    return;
                }
                
                var presenter = new TemplateCubePresenter(view.Value, cubeConfig, m_View.ScrollRect, OnCubeStartDrag);
                presenter.Initialize();
                m_Cubes.Add(presenter);
            }
        }

        private void OnCubeStartDrag(TemplateCubePresenter templateCubePresenter)
        {
            m_OnStartDrag?.Invoke(templateCubePresenter);
        }
    }
}