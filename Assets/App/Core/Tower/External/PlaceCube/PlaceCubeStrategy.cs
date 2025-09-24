using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Animations;
using App.Core.Tower.External.Data;
using App.Core.Tower.External.PlaceCube.DropChecker;
using App.Core.Tower.External.Presenter;
using UnityEngine;

namespace App.Core.Tower.External.PlaceCube
{
    public class PlaceCubeStrategy
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerDataController m_DataController;
        private readonly TowerPresenter m_TowerPresenter;
        private readonly Camera m_Camera;

        private TowerView m_TowerView;
        private CubePlacer m_CubePlacer;
        private JumpCubeAnimation m_JumpCubeAnimation;
        private CubeIntersectionChecker m_CubeIntersectionChecker;
        private IDropOnTowerChecker m_DropOnTowerChecker;

        public PlaceCubeStrategy(
            ICoreUIController coreUIController, 
            TowerDataController dataController,
            TowerPresenter towerPresenter)
        {
            m_CoreUIController = coreUIController;
            m_DataController = dataController;
            m_TowerPresenter = towerPresenter;

            m_Camera = Camera.main;
        }
        
        public void Initialize()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }
            m_TowerView = view.Value.TowerView;
            m_CubeIntersectionChecker = new CubeIntersectionChecker(m_TowerView);
            m_CubePlacer = new CubePlacer(m_CoreUIController, m_TowerPresenter, m_CubeIntersectionChecker);
            m_CubePlacer.Initialize(m_TowerView);
            m_JumpCubeAnimation = new JumpCubeAnimation();
            m_DropOnTowerChecker = new DropOnTowerChecker(m_TowerPresenter, m_Camera);
        }
        
        public DropOnTowerStatus Place(CubeView view, CubeConfig config)
        {
            if (!m_CubeIntersectionChecker.IsRectOnBackground(view))
            {
                return DropOnTowerStatus.NotIntersected;
            }
            
            var cubes = m_DataController.GetCubes();
            if (cubes.Count <= 0)
            {
                return m_CubePlacer.PlaceFirstCube(view, config);
            }
            
            if (!m_DropOnTowerChecker.CanDrop(view, config))
            {
                return DropOnTowerStatus.NotIntersected;
            }
            
            var status = m_CubePlacer.PlaceNextCube(view, config);
            if (status == DropOnTowerStatus.Added)
            {
                var lastPresenter = m_TowerPresenter.GetLastCube();
                m_JumpCubeAnimation.PlayJumpAnimation(view, lastPresenter, lastPresenter.View.RectTransform.position);
            }
            
            return status;
        }
    }
}