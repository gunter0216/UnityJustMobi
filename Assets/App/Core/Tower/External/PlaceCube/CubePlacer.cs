using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Cube;
using App.Core.Tower.External.Data;
using App.Core.Tower.External.Presenter;
using UnityEngine;

namespace App.Core.Tower.External.PlaceCube
{
    public class CubePlacer
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerPresenter m_TowerPresenter;
        private TowerView m_TowerView;

        public CubePlacer(ICoreUIController coreUIController, TowerPresenter towerPresenter)
        {
            m_CoreUIController = coreUIController;
            m_TowerPresenter = towerPresenter;
        }

        public void Initialize(TowerView towerView)
        {
            m_TowerView = towerView;
        }

        public DropOnTowerStatus PlaceFirstCube(CubeView view, CubeConfig config)
        {
            var position = view.RectTransform.position;
            var placeCube = PlaceCube(new Vector3(position.x, position.y, 0), config);
            return placeCube.HasValue ? DropOnTowerStatus.Added : DropOnTowerStatus.NotIntersected;
        }

        public DropOnTowerStatus PlaceNextCube(CubeView view, CubeConfig config)
        {
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

            return presenter.HasValue ? DropOnTowerStatus.Added : DropOnTowerStatus.NotIntersected;
        }

        private Optional<TowerCubePresenter> PlaceCube(Vector3 position, CubeConfig config)
        {
            var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config);
            if (!cubeView.HasValue)
            {
                Debug.LogError("[CubePlacer] In method PlaceCube, error create CubeView.");
                return Optional<TowerCubePresenter>.Fail();
            }

            cubeView.Value.transform.position = position;
            var cubeData = new CubeData
            {
                Key = config.Key,
                PositionX = position.x,
                PositionY = position.y
            };

            var presenter = m_TowerPresenter.CreateCubePresenter(cubeData, config, cubeView.Value);
            return Optional<TowerCubePresenter>.Success(presenter);
        }

        private bool CanPlaceCubeInPosition(Vector3 newPosition)
        {
            var last = m_TowerPresenter.GetLastCube();
            var rectTransform = last.View.RectTransform;
            var lastPosition = rectTransform.position;
            rectTransform.position = newPosition;
            rectTransform.position = lastPosition;
            return true;
        }

        private Vector3 GetNextCubePosition()
        {
            var last = m_TowerPresenter.GetLastCube();
            var rectTransform = last.View.RectTransform;
            var data = last.TowerCube.Data;
            var position = new Vector3(data.PositionX, data.PositionY, 0);
            var newPosition = position + Vector3.up * (rectTransform.rect.height * rectTransform.lossyScale.y);
            return newPosition;
        }

        private float GetHalfCubeWidth()
        {
            var last = m_TowerPresenter.GetLastCube();
            var rectTransform = last.View.RectTransform;
            var halfWidth = (rectTransform.rect.width * rectTransform.lossyScale.x) * 0.5f;
            return halfWidth;
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
                    break;
                }
            }
            return newPosition;
        }
    }
}

