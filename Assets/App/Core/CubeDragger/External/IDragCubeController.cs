using App.Core.CubesPanel.External.Presenter;

namespace App.Core.CubeDragger.External
{
    public interface IDragCubeController
    {
        void OnCubeStartDrag(TemplateCubePresenter templateCubePresenter);
    }
}