using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Audio.External;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Core.External.Config;
using App.Core.Core.External.Presenter;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External
{
    public class CubesPanelController : IInitSystem, IDisposable
    {
        private readonly ISceneManager m_SceneManager;
        private readonly ISoundManager m_SoundManager;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICubesController m_CubesController;
        private readonly ICoreUIController m_CoreUIController;
        
        private CubesPanelPresenter m_Presenter;

        public CubesPanelController(
            ISceneManager sceneManager, 
            ISoundManager soundManager, 
            ISpriteLoader spriteLoader, 
            ICubesController cubesController, 
            ICoreUIController coreUIController)
        {
            m_SceneManager = sceneManager;
            m_SoundManager = soundManager;
            m_SpriteLoader = spriteLoader;
            m_CubesController = cubesController;
            m_CoreUIController = coreUIController;
        }

        public void Init()
        {
            m_Presenter = new CubesPanelPresenter(
                m_SoundManager,
                m_CubesController,
                m_SpriteLoader,
                m_CoreUIController);
            if (!m_Presenter.Initialize())
            {
                Debug.LogError($"Cant initialize");
            }
        }

        public void Dispose()
        {
            m_Presenter?.Dispose();
        }
    }
}