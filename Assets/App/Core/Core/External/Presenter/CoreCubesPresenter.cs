using App.Core.Core.External.Config;
using App.Core.Core.External.View;

namespace App.Core.Core.External.Presenter
{
    public class CoreCubesPresenter
    {
        private readonly CoreCubesView m_View;
        private readonly CoreConfigController m_ConfigController;

        public CoreCubesPresenter(CoreCubesView view, CoreConfigController configController)
        {
            m_View = view;
            m_ConfigController = configController;
        }

        public void Initialize()
        {
            
        }
    }
}