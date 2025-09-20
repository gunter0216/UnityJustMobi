using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Audio.External;
using App.Core.Canvases.External;
using App.Core.Main.External.Presenter.Fabric;
using App.Core.Main.External.View;
using UnityEngine;

namespace App.Core.Main.External.Presenter
{
    public class CorePresenter : IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISoundManager m_SoundManager;
        
        private CoreView m_View;

        public CorePresenter(
            IAssetManager assetManager,
            ICanvas canvas,
            ISoundManager soundManager)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SoundManager = soundManager;
        }

        public bool Initialize()
        {
            if (!CreateView())
            {
                return false;
            }

            return true;
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