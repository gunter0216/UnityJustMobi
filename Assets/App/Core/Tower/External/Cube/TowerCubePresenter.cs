using App.Core.CoreUI.External.View;

namespace App.Core.Tower.External
{
    public class TowerCubePresenter
    {
        private readonly TowerCube m_TowerCube;
        private readonly CubeView m_View;

        public TowerCubePresenter(TowerCube towerCube, CubeView view)
        {
            m_TowerCube = towerCube;
            m_View = view;
        }
    }
}