using App.Core.Core.External.Config;
using App.Core.Core.External.View;

namespace App.Core.Core.External.Presenter
{
    public interface IHoleController
    {
        void DropInHole(CubeView view, CubeConfig config);
    }
}