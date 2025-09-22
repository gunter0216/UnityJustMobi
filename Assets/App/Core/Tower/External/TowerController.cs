using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External.Config;

namespace App.Core.Tower.External
{
    public class TowerController : IInitSystem, ITowerController 
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly IMessageController m_MessageController;
        
        private TowerView m_View;

        public TowerController(ICoreUIController coreUIController, IMessageController messageController)
        {
            m_CoreUIController = coreUIController;
            m_MessageController = messageController;
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