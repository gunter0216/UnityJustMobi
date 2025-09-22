using System;
using App.Common.Audio.External;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
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
        private readonly IDragCubeController m_DragCubeController;
        
        private CubesPanelPresenter m_Presenter;

        public CubesPanelController(
            ISceneManager sceneManager, 
            ISoundManager soundManager, 
            ISpriteLoader spriteLoader, 
            ICubesController cubesController, 
            ICoreUIController coreUIController, 
            IDragCubeController dragCubeController)
        {
            m_SceneManager = sceneManager;
            m_SoundManager = soundManager;
            m_SpriteLoader = spriteLoader;
            m_CubesController = cubesController;
            m_CoreUIController = coreUIController;
            m_DragCubeController = dragCubeController;
        }

        public void Init()
        {
            m_Presenter = new CubesPanelPresenter(
                m_SoundManager,
                m_CubesController,
                m_SpriteLoader,
                m_CoreUIController,
                m_DragCubeController);
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