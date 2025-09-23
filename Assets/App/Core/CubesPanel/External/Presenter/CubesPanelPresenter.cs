using System;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.CubesPanel.External.Presenter
{
    public class CubesPanelPresenter : IDisposable
    {
        private readonly ICubesController m_CubesController;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICoreUIController m_CoreUIController;
        private event Action<TemplateCubePresenter> m_OnCubeStartDrag;

        private CoreView m_View;
        private CubesPresenter m_CubesPresenter;

        public CubesPanelPresenter(
            ICubesController cubesController, 
            ISpriteLoader spriteLoader,
            ICoreUIController coreUIController,
            Action<TemplateCubePresenter> onCubeStartDrag)
        {
            m_CubesController = cubesController;
            m_SpriteLoader = spriteLoader;
            m_CoreUIController = coreUIController;
            m_OnCubeStartDrag = onCubeStartDrag;
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
            m_OnCubeStartDrag?.Invoke(templateCubePresenter);
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