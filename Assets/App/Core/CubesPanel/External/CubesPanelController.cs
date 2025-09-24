using System;
using App.Common.Events.External;
using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.Cubes.External;
using App.Core.CubesPanel.External.Presenter;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.CubesPanel.External
{
    /// <summary>
    /// Отвечает за панель кубов. Поднятие кубика, но не его перемещение, только тригерит событие
    /// </summary>
    public class CubesPanelController : IInitSystem
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICubesController m_CubesController;
        private readonly ICoreUIController m_CoreUIController;
        private readonly IEventManager m_EventManager;
        
        private CubesPanelPresenter m_Presenter;

        public CubesPanelController(
            ISpriteLoader spriteLoader, 
            ICubesController cubesController, 
            ICoreUIController coreUIController, 
            IEventManager eventManager)
        {
            m_SpriteLoader = spriteLoader;
            m_CubesController = cubesController;
            m_CoreUIController = coreUIController;
            m_EventManager = eventManager;
        }

        public void Init()
        {
            m_Presenter = new CubesPanelPresenter(
                m_CubesController,
                m_SpriteLoader,
                m_CoreUIController,
                OnCubeStartDrag);
            if (!m_Presenter.Initialize())
            {
                Debug.LogError($"Cant initialize");
            }
        }

        private void OnCubeStartDrag(TemplateCubePresenter presenter)
        {
            m_EventManager.Trigger(new TemplateCubeStartDragEvent(presenter));
        }
    }
}