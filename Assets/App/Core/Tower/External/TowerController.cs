using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Events.External;
using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using App.Core.Tower.External.Events;
using UnityEngine;

namespace App.Core.Tower.External
{
    public class TowerController : IInitSystem, ITowerController 
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly IMessageController m_MessageController;
        private readonly IDataManager m_DataManager;
        private readonly ICubesController m_CubesController;
        private readonly IEventManager m_EventManager;

        private PlaceCubeStrategy m_PlaceCubeStrategy;
        private TowerDataController m_DataController;
        private TowerView m_View;
        
        private List<TowerCubePresenter> m_CubePresenters;

        public TowerController(
            ICoreUIController coreUIController, 
            IMessageController messageController, 
            IDataManager dataManager, 
            ICubesController cubesController, 
            IEventManager eventManager)
        {
            m_CoreUIController = coreUIController;
            m_MessageController = messageController;
            m_DataManager = dataManager;
            m_CubesController = cubesController;
            m_EventManager = eventManager;
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
            
            m_PlaceCubeStrategy = new PlaceCubeStrategy(
                m_CoreUIController, 
                m_DataController, 
                m_CubesController,
                TowerCubeStartDrag);
            m_PlaceCubeStrategy.Initialize();
        }

        private void TowerCubeStartDrag(TowerCubePresenter presenter)
        {
            m_EventManager.Trigger(new TowerCubeStartDragEvent(presenter));
        }

        public DropOnTowerStatus DropCubeOnTower(CubeView view, CubeConfig config)
        {
            var status = m_PlaceCubeStrategy.Place(view, config);
            if (status == DropOnTowerStatus.Added)
            {
                m_MessageController.ShowMessage("Placed");
            } 
            else if (status == DropOnTowerStatus.TowerIsMax)
            {
                m_MessageController.ShowMessage("Max tower");
            }
            
            return status;
        }
    }
}