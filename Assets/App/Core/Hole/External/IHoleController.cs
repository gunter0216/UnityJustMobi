using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;

namespace App.Core.Hole.External
{
    public interface IHoleController
    {
        bool DropInHole(CubeView view, CubeConfig config);
    }
}