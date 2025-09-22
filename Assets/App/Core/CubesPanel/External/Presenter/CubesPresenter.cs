using System;
using System.Collections.Generic;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CubesPresenter
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly CubeViewCreator m_CubeViewCreator;
        private readonly CoreCubesView m_View;
        private readonly ICubesController m_CubesController;
        private readonly Action<TemplateCubePresenter> m_OnStartDrag;

        private List<TemplateCubePresenter> m_Cubes;

        public CubesPresenter(
            CubeViewCreator cubeViewCreator, 
            CoreCubesView view,
            ICubesController cubesController,
            ISpriteLoader spriteLoader,
            Action<TemplateCubePresenter> onStartDrag)
        {
            m_CubeViewCreator = cubeViewCreator;
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
                var view = m_CubeViewCreator.Create(m_View.CubesContent);
                if (!view.HasValue)
                {
                    Debug.LogError("Cant create cube view");
                    return;
                }
                
                var presenter = new TemplateCubePresenter(m_SpriteLoader, view.Value, cubeConfig, m_View.ScrollRect, OnCubeStartDrag);
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