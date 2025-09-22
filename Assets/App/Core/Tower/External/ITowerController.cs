using App.Core.Core.External.Config;
using App.Core.Core.External.View;

namespace App.Core.Tower.External
{
    public interface ITowerController
    {
        bool DropInTower(CubeView view, CubeConfig config);
    }
}