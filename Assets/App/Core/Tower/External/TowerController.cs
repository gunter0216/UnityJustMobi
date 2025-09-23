using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using UnityEngine;

namespace App.Core.Tower.External
{
    public class TowerController : IInitSystem, ITowerController 
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly IMessageController m_MessageController;
        private readonly IDataManager m_DataManager;
        private readonly ICubesController m_CubesController;

        private PlaceCubeStrategy m_PlaceCubeStrategy;
        private TowerDataController m_DataController;
        private TowerView m_View;
        
        private List<TowerCubePresenter> m_CubePresenters;

        public TowerController(
            ICoreUIController coreUIController, 
            IMessageController messageController, 
            IDataManager dataManager, 
            ICubesController cubesController)
        {
            m_CoreUIController = coreUIController;
            m_MessageController = messageController;
            m_DataManager = dataManager;
            m_CubesController = cubesController;
        }

        public void Init()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }

            m_View = view.Value.TowerView;

            m_DataController = new TowerDataController(m_DataManager);
            if (!m_DataController.Initialize())
            {
                Debug.LogError("[TowerController] In method Init, error load TowerData.");
                return;
            }
            
            m_PlaceCubeStrategy = new PlaceCubeStrategy(m_CoreUIController, m_DataController);
            m_PlaceCubeStrategy.Initialize();
        }

        public bool DropInTower(CubeView view, CubeConfig config)
        {
            return m_PlaceCubeStrategy.Place(view, config);
        }
    }
}