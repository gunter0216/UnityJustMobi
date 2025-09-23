using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.External;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using App.Core.Utility.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Core.Tower.External
{
    public class PlaceCubeStrategy
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerDataController m_DataController;
        private readonly ICubesController m_CubesController;
        private readonly Camera m_Camera;
        private event Action<TowerCubePresenter> m_OnTowerCubeStartDrag;

        private List<TowerCubePresenter> m_CubePresenters;
        private TowerView m_TowerView;

        public PlaceCubeStrategy(
            ICoreUIController coreUIController, 
            TowerDataController dataController, 
            ICubesController cubesController,
            Action<TowerCubePresenter> onTowerCubeStartDrag)
        {
            m_CoreUIController = coreUIController;
            m_DataController = dataController;
            m_CubesController = cubesController;
            m_OnTowerCubeStartDrag = onTowerCubeStartDrag;

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
        
        public DropOnTowerStatus Place(CubeView view, CubeConfig config)
        {
            var cubes = m_DataController.GetCubes();
            if (cubes.Count <= 0)
            {
                if (!RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform))
                {
                    return DropOnTowerStatus.TowerIsMax;
                }
                
                var position = view.RectTransform.position;
                return PlaceCube(new Vector3(position.x, position.y, 0), config)
                    ? DropOnTowerStatus.Added
                    : DropOnTowerStatus.NotIntersected;
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
                        return DropOnTowerStatus.TowerIsMax;
                    }
                    
                    var halfWidth = (rectTransform.rect.width * rectTransform.lossyScale.x) * 0.5f;
                    const int maxAttempts = 1000;
                    for (int i = 0; i < maxAttempts; ++i)
                    {
                        var randomOffset = Random.Range(-halfWidth, halfWidth);
                        view.RectTransform.SetPositionX(newPosition.x + randomOffset);
                        if (RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform))
                        {
                            newPosition.x += randomOffset;
                            break;
                        }
                    }
                    
                    view.RectTransform.position = dragCubePosition;
                    
                    return PlaceCube(newPosition, config) ? DropOnTowerStatus.Added : DropOnTowerStatus.NotIntersected;
                }
            }

            return DropOnTowerStatus.NotIntersected;
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
                
                var presenter = CreateCubePresenter(cubeData, config.Value, cubeView.Value);
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
            
            var presenter = CreateCubePresenter(cubeData, config, cubeView.Value);
            m_CubePresenters.Add(presenter);
            m_DataController.AddCube(cubeData);

            return true;
        }

        private TowerCubePresenter CreateCubePresenter(CubeData data, CubeConfig config, CubeView cubeView)
        {
            var towerCube = new TowerCube(data, config);
            var presenter = new TowerCubePresenter(towerCube, cubeView, OnTowerCubeStartDrag);
            presenter.Initialize();
            return presenter;
        }

        private void OnTowerCubeStartDrag(TowerCubePresenter presenter)
        {
            m_OnTowerCubeStartDrag?.Invoke(presenter);
            m_CoreUIController.DestroyCubeView(presenter.View);
            if (!m_CubePresenters.Remove(presenter))
            {
                foreach (var cubePresenter in m_CubePresenters)
                {
                    Debug.LogError($"> {cubePresenter.TowerCube.Data.Key}");   
                }
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove TowerCubePresenter from list.");
            }

            if (!m_DataController.RemoveCube(presenter.TowerCube.Data))
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove CubeData from TowerDataController.");
            }
        }
    }
}