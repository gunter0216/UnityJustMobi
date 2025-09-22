using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.Hole.External.Animation;

namespace App.Core.Hole.External
{
    public class HoleController : IInitSystem, IHoleController 
    {
        private readonly ICoreUIController m_CoreUIController;
        
        private HoleView m_View;
        private CubeHoleAnimation m_CubeHoleAnimation;

        public HoleController(ICoreUIController coreUIController)
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

            m_View = view.Value.HoleView;
            m_CubeHoleAnimation = new CubeHoleAnimation(m_CoreUIController, m_View);
        }

        public bool DropInHole(CubeView view, CubeConfig config)
        {
            return m_CubeHoleAnimation.DropInHole(view, config);
        }
    }
}