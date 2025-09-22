using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;

namespace App.Core.Tower.External
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