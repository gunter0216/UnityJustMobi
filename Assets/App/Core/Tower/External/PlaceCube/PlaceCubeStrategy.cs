using System.Collections.Generic;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using UnityEngine;

namespace App.Core.Tower.External
{
    public class PlaceCubeStrategy
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerDataController m_DataController;
        
        private List<TowerCubePresenter> m_CubePresenters;
        private TowerView m_TowerView;

        public PlaceCubeStrategy(ICoreUIController coreUIController, TowerDataController dataController)
        {
            m_CoreUIController = coreUIController;
            m_DataController = dataController;
        }
        
        public void Initialize()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }
            
            m_TowerView = view.Value.TowerView;
            m_CubePresenters = new List<TowerCubePresenter>();
        }

        public bool Place(CubeView view, CubeConfig config)
        {
            var cubes = m_DataController.GetCubes();
            if (cubes.Count <= 0)
            {
                PlaceCube(view.RectTransform.position, config);
            }
            
            return true;
        }

        private void PlaceCube(Vector3 position, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("[PlaceCubeStrategy] In method PlaceCube, error create CubeView.");
                return;
            }
            
            cubeView.Value.transform.position = position;
            var cubeData = new CubeData
            {
                Key = config.Key,
                PositionX = position.x,
                PositionY = position.y
            };
            
            var towerCube = new TowerCube(cubeData, config);
            var presenter = new TowerCubePresenter(towerCube, cubeView.Value);
            m_CubePresenters.Add(presenter);
            m_DataController.AddCube(cubeData);
        }
    }
}