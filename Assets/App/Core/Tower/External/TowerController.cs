using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using App.Core.Tower.External;

namespace App.Core.Core.External.Presenter
{
    public class TowerController : IInitSystem, ITowerController 
    {
        private readonly ICoreUIController m_CoreUIController;
        private TowerView m_View;

        public TowerController(ICoreUIController coreUIController)
        {
            m_CoreUIController = coreUIController;
        }

        public void Init()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }

            m_View = view.Value.TowerView;
        }

        public bool DropInTower(CubeView view, CubeConfig config)
        {
            return true;
        }
    }
}