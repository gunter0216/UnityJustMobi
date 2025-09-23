using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;

namespace App.Core.Tower.External
{
    public interface ITowerController
    {
        DropTowerStatus DropInTower(CubeView view, CubeConfig config);
    }
}