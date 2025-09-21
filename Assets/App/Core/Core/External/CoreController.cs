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
    public class CoreController : IInitSystem, IDisposable
    {
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        private readonly IDataManager m_DataManager;
        private readonly ISceneManager m_SceneManager;
        private readonly ISoundManager m_SoundManager;
        private readonly IConfigLoader m_ConfigLoader;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private CorePresenter m_Presenter;
        private CoreConfigController m_ConfigController;

        public CoreController(
            MainCanvas mainCanvas, 
            IAssetManager assetManager, 
            IDataManager dataManager, 
            ISceneManager sceneManager, 
            ISoundManager soundManager, 
            IConfigLoader configLoader, 
            ISpriteLoader spriteLoader)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
            m_DataManager = dataManager;
            m_SceneManager = sceneManager;
            m_SoundManager = soundManager;
            m_ConfigLoader = configLoader;
            m_SpriteLoader = spriteLoader;
        }

        public void Init()
        {
            m_ConfigController = new CoreConfigController(m_ConfigLoader);
            if (!m_ConfigController.Initialize())
            {
                Debug.LogError($"Cant initialize CoreConfigController");
                return;
            }
            
            m_Presenter = new CorePresenter(
                m_AssetManager, 
                m_MainCanvas,
                m_SoundManager,
                m_ConfigController,
                m_SpriteLoader);
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