using App.Core.CoreUI.External.View;
using App.Core.Utility.External;

namespace App.Core.Tower.External
{
    public class TowerCubePresenter
    {
        private readonly TowerCube m_TowerCube;
        private readonly CubeView m_View;
        
        public TowerCube TowerCube => m_TowerCube;
        public CubeView View => m_View;

        public TowerCubePresenter(TowerCube towerCube, CubeView view)
        {
            m_TowerCube = towerCube;
            m_View = view;
        }

        public bool Intersect(CubeView view)
        {
            return RectBoundsChecker.AreRectsIntersectingByCorners(m_View.RectTransform, view.RectTransform);
        }
    }
}