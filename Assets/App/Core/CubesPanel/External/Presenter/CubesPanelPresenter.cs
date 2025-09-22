using System;
using App.Common.Audio.External;
using App.Core.Core.External.Config;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CubesPanelPresenter : IDisposable
    {
        private readonly ISoundManager m_SoundManager;
        private readonly ICubesController m_CubesController;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICoreUIController m_CoreUIController;
        private readonly IDragCubeController m_DragCubeController;

        private CoreView m_View;
        private CubesPresenter m_CubesPresenter;

        public CubesPanelPresenter(
            ISoundManager soundManager,
            ICubesController cubesController, 
            ISpriteLoader spriteLoader,
            ICoreUIController coreUIController, 
            IDragCubeController dragCubeController)
        {
            m_SoundManager = soundManager;
            m_CubesController = cubesController;
            m_SpriteLoader = spriteLoader;
            m_CoreUIController = coreUIController;
            m_DragCubeController = dragCubeController;
        }

        public bool Initialize()
        {
            if (!GetView())
            {
                return false;
            }

            InitView();

            return true;
        }

        private void InitView()
        {
            m_CubesPresenter = new CubesPresenter(m_CoreUIController, m_View.CubesView, m_CubesController, m_SpriteLoader, OnCubeStartDrag);
            m_CubesPresenter.Initialize();
        }

        private void OnCubeStartDrag(TemplateCubePresenter templateCubePresenter)
        {
            m_DragCubeController.OnCubeStartDrag(templateCubePresenter);
        }

        private bool GetView()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                Debug.LogError("Cant get CoreView from CoreUIController");
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