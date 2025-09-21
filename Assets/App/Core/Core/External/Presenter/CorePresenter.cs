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
            m_CubesPresenter = new CoreCubesPresenter(m_View.CubesView, m_ConfigController, m_SpriteLoader);
            m_CubesPresenter.Initialize();
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
        }
    }
}