using App.Core.CoreUI.External.View;
using App.Core.Utility.External;

namespace App.Core.Tower.External.PlaceCube
{
    public class CubeIntersectionChecker
    {
        private readonly TowerView m_TowerView;

        public CubeIntersectionChecker(TowerView towerView)
        {
            m_TowerView = towerView;
        }

        public bool IsRectOnBackground(CubeView view)
        {
            return RectBoundsChecker.IsRectCompletelyInside(view.RectTransform, m_TowerView.RectTransform);
        }
    }
}

