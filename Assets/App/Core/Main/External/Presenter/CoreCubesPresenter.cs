using App.Core.Main.External.Config;
using App.Core.Main.External.View;

namespace App.Core.Main.External.Presenter
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