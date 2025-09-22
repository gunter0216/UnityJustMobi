using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External;
using App.Core.Cubes.External.Config;
using App.Core.Hole.External.Animation;

namespace App.Core.Hole.External
{
    public class HoleController : IInitSystem, IHoleController 
    {
        private readonly ICoreUIController m_CoreUIController;
        private readonly IMessageController m_MessageController;
        
        private HoleView m_View;
        private CubeHoleAnimation m_CubeHoleAnimation;

        public HoleController(ICoreUIController coreUIController, IMessageController messageController)
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

            m_View = view.Value.HoleView;
            m_CubeHoleAnimation = new CubeHoleAnimation(m_CoreUIController, m_View);
        }

        public bool DropInHole(CubeView view, CubeConfig config)
        {
            var success = m_CubeHoleAnimation.DropInHole(view, config);
            if (success)
            {
                m_MessageController.ShowMessage("Drop in hole!");
            }
            
            return success;
        }
    }
}