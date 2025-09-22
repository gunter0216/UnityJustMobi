using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CoreCubesPresenter
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly CubeViewCreator m_CubeViewCreator;
        private readonly CoreCubesView m_View;
        private readonly CoreConfigController m_ConfigController;
        private readonly Action<CubePresenter> m_OnStartDrag;

        private List<CubePresenter> m_Cubes;

        public CoreCubesPresenter(
            CubeViewCreator cubeViewCreator, 
            CoreCubesView view,
            CoreConfigController configController,
            ISpriteLoader spriteLoader,
            Action<CubePresenter> onStartDrag)
        {
            m_CubeViewCreator = cubeViewCreator;
            m_View = view;
            m_ConfigController = configController;
            m_SpriteLoader = spriteLoader;
            m_OnStartDrag = onStartDrag;
        }

        public void Initialize()
        {
            var cubes = m_ConfigController.GetCubes();
            m_Cubes = new List<CubePresenter>(cubes.Count);
            foreach (var cubeConfig in cubes)
            {
                var view = m_CubeViewCreator.Create(m_View.CubesContent);
                if (!view.HasValue)
                {
                    Debug.LogError("Cant create cube view");
                    return;
                }
                
                var presenter = new CubePresenter(m_SpriteLoader, view.Value, cubeConfig, m_View.ScrollRect, OnCubeStartDrag);
                presenter.Initialize();
                m_Cubes.Add(presenter);
            }
        }

        private void OnCubeStartDrag(CubePresenter cubePresenter)
        {
            m_OnStartDrag?.Invoke(cubePresenter);
        }
    }
}