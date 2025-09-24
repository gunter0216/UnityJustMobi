using App.Core.CubesPanel.External.Presenter;

namespace App.Core.CubesPanel.External.Events
{
    public class TemplateCubeStartDragEvent
    {
        private readonly TemplateCubePresenter m_Presenter;

        public TemplateCubePresenter Presenter => m_Presenter;

        public TemplateCubeStartDragEvent(TemplateCubePresenter presenter)
        {
            m_Presenter = presenter;
        }
    }
}