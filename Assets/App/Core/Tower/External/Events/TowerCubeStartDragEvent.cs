using App.Core.Tower.External.Cube;

namespace App.Core.Tower.External.Events
{
    public class TowerCubeStartDragEvent
    {
        private readonly TowerCubePresenter m_Presenter;

        public TowerCubePresenter Presenter => m_Presenter;

        public TowerCubeStartDragEvent(TowerCubePresenter presenter)
        {
            m_Presenter = presenter;
        }
    }
}