using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Audio.External;
using App.Core.Canvases.External;
using App.Core.Core.External.Config;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CorePresenter : IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISoundManager m_SoundManager;
        private readonly CoreConfigController m_ConfigController;
        private readonly ISpriteLoader m_SpriteLoader;

        private CoreView m_View;
        private CoreCubesPresenter m_CubesPresenter;
        private DragCubeController m_DragCubeController;

        public CorePresenter(
            IAssetManager assetManager,
            ICanvas canvas,
            ISoundManager soundManager,
            CoreConfigController configController, 
            ISpriteLoader spriteLoader)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SoundManager = soundManager;
            m_ConfigController = configController;
            m_SpriteLoader = spriteLoader;
        }

        public bool Initialize()
        {
            if (!CreateView())
            {
                return false;
            }

            InitView();

            return true;
        }

        private void InitView()
        {
            var cubeViewCreator = new CubeViewCreator(m_View.CubesView);
            m_DragCubeController = new DragCubeController(cubeViewCreator, m_SpriteLoader, m_View);
            m_DragCubeController.Initialize();
            
            m_CubesPresenter = new CoreCubesPresenter(cubeViewCreator, m_View.CubesView, m_ConfigController, m_SpriteLoader, OnCubeStartDrag);
            m_CubesPresenter.Initialize();
        }
        
        private void OnCubeStartDrag(CubePresenter cubePresenter)
        {
            m_DragCubeController.OnCubeStartDrag(cubePresenter);
        }

        private bool CreateView()
        {
            var menuViewCreator = new CoreViewCreator(m_AssetManager, m_Canvas);
            var view = menuViewCreator.Create();
            if (!view.HasValue)
            {
                Debug.LogError("Cant create MenuView");
                return false;
            }
            
            m_View = view.Value;
            return true;
        }

        public void Dispose()
        {
            m_DragCubeController?.Dispose();
        }
    }
}