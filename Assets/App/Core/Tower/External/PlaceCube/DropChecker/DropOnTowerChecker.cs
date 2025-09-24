using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Presenter;
using UnityEngine;

namespace App.Core.Tower.External.PlaceCube.DropChecker
{
    public class DropOnTowerChecker : IDropOnTowerChecker
    {
        private readonly TowerPresenter m_TowerPresenter;
        private readonly Camera m_Camera;

        public DropOnTowerChecker(TowerPresenter towerPresenter, Camera camera)
        {
            m_TowerPresenter = towerPresenter;
            m_Camera = camera;
        }

        public bool CanDrop(CubeView view, CubeConfig config)
        {
            Vector2 mousePosition = Input.mousePosition;
            foreach (var cubePresenter in m_TowerPresenter.GetCubes())
            {
                var rect = cubePresenter.View.RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePosition, m_Camera))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}