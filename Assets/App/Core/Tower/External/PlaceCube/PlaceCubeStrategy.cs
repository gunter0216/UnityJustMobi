using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.External;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using App.Core.Utility.External;
using UnityEngine;

namespace App.Core.Tower.External
{
    public class PlaceCubeStrategy
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerDataController m_DataController;
        private readonly ICubesController m_CubesController;
        private readonly Camera m_Camera;

        private List<TowerCubePresenter> m_CubePresenters;
        private TowerView m_TowerView;

        public PlaceCubeStrategy(
            ICoreUIController coreUIController, 
            TowerDataController dataController, 
            ICubesController cubesController)
        {
            m_CoreUIController = coreUIController;
            m_DataController = dataController;
            m_CubesController = cubesController;

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
            m_CubePresenters = new List<TowerCubePresenter>();

            LoadTowerFromData();
        }
        
        public DropTowerStatus Place(CubeView view, CubeConfig config)
        {
            var cubes = m_DataController.GetCubes();
            if (cubes.Count <= 0)
            {
                if (!RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform))
                {
                    return DropTowerStatus.TowerIsMax;
                }
                
                var position = view.RectTransform.position;
                return PlaceCube(new Vector3(position.x, position.y, 0), config)
                    ? DropTowerStatus.Added
                    : DropTowerStatus.NotIntersected;
            }

            Vector2 mousePosition = Input.mousePosition;
            foreach (var cubePresenter in m_CubePresenters)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cubePresenter.View.RectTransform, mousePosition, m_Camera))
                {
                    var last = m_CubePresenters.Last();
                    var rectTransform = last.View.RectTransform;
                    var dragCubePosition = view.RectTransform.position;
                    var newPosition = rectTransform.position + Vector3.up * (rectTransform.rect.height * rectTransform.lossyScale.y);
                    view.RectTransform.position = newPosition;
                    
                    if (!RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform))
                    {
                        view.RectTransform.position = dragCubePosition;
                        return DropTowerStatus.TowerIsMax;
                    }
                    
                    view.RectTransform.position = dragCubePosition;
                    
                    return PlaceCube(newPosition, config) ? DropTowerStatus.Added : DropTowerStatus.NotIntersected;
                }
            }

            return DropTowerStatus.NotIntersected;
        }
        
        private void LoadTowerFromData()
        {
            foreach (var cubeData in m_DataController.GetCubes())
            {
                var config = m_CubesController.GetCubeConfig(cubeData.Key);
                if (!config.HasValue)
                {
                    Debug.LogError($"[PlaceCubeStrategy] In method LoadTowerFromData, not found CubeConfig by key {cubeData.Key}.");
                    continue;
                }
                
                var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config.Value);
                if (!cubeView.HasValue)
                {
                    Debug.LogError("[PlaceCubeStrategy] In method PlaceCube, error create CubeView.");
                    return;
                }
                
                var towerCube = new TowerCube(cubeData, config.Value);
                var presenter = new TowerCubePresenter(towerCube, cubeView.Value);
                m_CubePresenters.Add(presenter);
            }
            
            GlobalCoroutineProvider.DoCoroutine(UpdateCubesPosition());
        }

        private IEnumerator UpdateCubesPosition()
        {
            yield return new WaitForEndOfFrame();
            foreach (var cubePresenter in m_CubePresenters)
            {
                var data = cubePresenter.TowerCube.Data;
                cubePresenter.View.SetGlobalPosition(new Vector3(data.PositionX, data.PositionY, 0));
            }
        }

        private bool PlaceCube(Vector3 position, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("[PlaceCubeStrategy] In method PlaceCube, error create CubeView.");
                return false; 
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

            return true;
        }
    }
}