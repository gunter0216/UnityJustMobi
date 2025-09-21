using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CoreCubesPresenter
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly CoreCubesView m_View;
        private readonly CoreConfigController m_ConfigController;

        private List<CubePresenter> m_Cubes;

        public CoreCubesPresenter(CoreCubesView view, CoreConfigController configController, ISpriteLoader spriteLoader)
        {
            m_View = view;
            m_ConfigController = configController;
            m_SpriteLoader = spriteLoader;
        }

        public void Initialize()
        {
            var cubes = m_ConfigController.GetCubes();
            m_Cubes = new List<CubePresenter>(cubes.Count);
            foreach (var cubeConfig in cubes)
            {
                var view = Object.Instantiate(m_View.CubeViewPrefab, m_View.CubesContent);
                var presenter = new CubePresenter(m_SpriteLoader, view, cubeConfig);
                presenter.Initialize();
                m_Cubes.Add(presenter);
            }
        }
    }
}