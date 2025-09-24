using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;

namespace App.Core.Tower.External.PlaceCube.DropChecker
{
    public interface IDropOnTowerChecker
    {
        bool CanDrop(CubeView view, CubeConfig config);
    }
}