using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Audio.External;
using App.Common.Data.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Main.External.Presenter;
using UnityEngine;

namespace App.Core.Main.External
{
    public class CoreController : IInitSystem, IDisposable
    {
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        private readonly IDataManager m_DataManager;
        private readonly ISceneManager m_SceneManager;
        private readonly ISoundManager m_SoundManager;
        
        private CorePresenter m_Presenter;

        public CoreController(
            MainCanvas mainCanvas, 
            IAssetManager assetManager, 
            IDataManager dataManager, 
            ISceneManager sceneManager, 
            ISoundManager soundManager)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
            m_DataManager = dataManager;
            m_SceneManager = sceneManager;
            m_SoundManager = soundManager;
        }

        public void Init()
        {
            m_Presenter = new CorePresenter(
                m_AssetManager, 
                m_MainCanvas,
                m_SoundManager);
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