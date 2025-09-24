using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.External;
using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;
using App.Core.Utility.External;
using DG.Tweening;
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
            if (!IsRectOnBackground(view))
            {
                return DropOnTowerStatus.NotIntersected;
            }
            
            var cubes = m_DataController.GetCubes();
            if (cubes.Count <= 0)
            {
                return PlaceFirstCube(view, config);
            }
            
            return PlaceNextCube(view, config);
        }
        
        private DropOnTowerStatus PlaceFirstCube(CubeView view, CubeConfig config)
        {
            var position = view.RectTransform.position;
            var placeCube = PlaceCube(new Vector3(position.x, position.y, 0), config);
            return placeCube.HasValue ? DropOnTowerStatus.Added : DropOnTowerStatus.NotIntersected;
        }

        private DropOnTowerStatus PlaceNextCube(CubeView view, CubeConfig config)
        {
            if (!IsDropOnTower(view))
            {
                return DropOnTowerStatus.NotIntersected;
            }
            
            var newPosition = GetNextCubePosition();
            if (!CanPlaceCubeInPosition(newPosition))
            {
                return DropOnTowerStatus.TowerIsMax;
            }

            newPosition = AddXOffset(newPosition);

            var presenter = PlaceCube(newPosition, config);
            if (!presenter.HasValue)
            {
                return DropOnTowerStatus.NotIntersected;
            }
            
            PlayJumpAnimation(view, presenter.Value, newPosition);

            return presenter.HasValue ? DropOnTowerStatus.Added : DropOnTowerStatus.NotIntersected;
        }

        private void PlayJumpAnimation(CubeView view, TowerCubePresenter presenter, Vector3 newPosition)
        {
            const float duration = 0.20f;
            var dragCubePosition = view.RectTransform.position;
            presenter.View.SetGlobalPosition(dragCubePosition);
            presenter.View.RectTransform.DOJump(newPosition, 1, 1, duration);
        }

        private Vector3 AddXOffset(Vector3 position)
        {
            const int maxAttempts = 1000;
            var halfWidth = GetHalfCubeWidth();
            var newPosition = position;
            for (int i = 0; i < maxAttempts; ++i)
            {
                var randomOffset = Random.Range(-halfWidth, halfWidth);
                newPosition.x = position.x + randomOffset;
                if (CanPlaceCubeInPosition(newPosition))
                {
                    newPosition.x += randomOffset;
                    break;
                }
            }

            return newPosition;
        }

        private bool CanPlaceCubeInPosition(Vector3 newPosition)
        {
            var last = m_CubePresenters.Last();
            var rectTransform = last.View.RectTransform;
            var lastPosition = rectTransform.position;
            rectTransform.position = newPosition;
            var isRectOnBackground = IsRectOnBackground(last.View);
            rectTransform.position = lastPosition;

            return isRectOnBackground;
        }

        private Vector3 GetNextCubePosition()
        {
            var last = m_CubePresenters.Last();
            var rectTransform = last.View.RectTransform;
            var data = last.TowerCube.Data;
            var position = new Vector3(data.PositionX, data.PositionY, 0);
            var newPosition = position + Vector3.up * (rectTransform.rect.height * rectTransform.lossyScale.y);
            return newPosition;
        }
        
        private float GetHalfCubeWidth()
        {
            var last = m_CubePresenters.Last();
            var rectTransform = last.View.RectTransform;
            var halfWidth = (rectTransform.rect.width * rectTransform.lossyScale.x) * 0.5f;
            return halfWidth;
        }

        private bool IsDropOnTower(CubeView view)
        {
            Vector2 mousePosition = Input.mousePosition;
            foreach (var cubePresenter in m_CubePresenters)
            {
                var rect = cubePresenter.View.RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePosition, m_Camera))
                {
                    return true;
                }
            }

            return false;
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

        private Optional<TowerCubePresenter> PlaceCube(Vector3 position, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("[PlaceCubeStrategy] In method PlaceCube, error create CubeView.");
                return Optional<TowerCubePresenter>.Fail(); 
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

            return Optional<TowerCubePresenter>.Success(presenter);
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
            var presenterIndex = m_CubePresenters.IndexOf(presenter);
            if (presenterIndex < 0)
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, not found TowerCubePresenter in list.");
                return;
            }
            
            m_OnTowerCubeStartDrag?.Invoke(presenter);

            PlayFallAnimation(presenterIndex);
            
            m_CoreUIController.DestroyCubeView(presenter.View);
            if (!m_CubePresenters.Remove(presenter))
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove TowerCubePresenter from list.");
            }

            if (!m_DataController.RemoveCube(presenter.TowerCube.Data))
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove CubeData from TowerDataController.");
            }
            
            presenter.Destroy();
        }

        private void PlayFallAnimation(int startIndex)
        {
            if (startIndex == m_CubePresenters.Count - 1)
            {
                return;
            }

            const float duration = 0.20f;
            for (int i = startIndex; i < m_CubePresenters.Count; ++i)
            {
                var cubePresenter = m_CubePresenters[i];
                var data = cubePresenter.TowerCube.Data;
                var rectTransform = cubePresenter.View.RectTransform;
                var previousPosition = new Vector3(data.PositionX, data.PositionY, 0);
                var newPosition = previousPosition +
                                  Vector3.down * (rectTransform.rect.height * rectTransform.lossyScale.y);
                rectTransform.DOMoveY(newPosition.y, duration);
                data.PositionX = newPosition.x;
                data.PositionY = newPosition.y;
            }
        }

        private bool IsRectOnBackground(CubeView view)
        {
            return RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform);
        }
    }
}