using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using UnityEngine;

namespace App.Core.CoreUI.External
{
    public interface ICoreUIController
    {
        Optional<CoreView> GetView();
        Optional<CubeView> CreateCubeView(Transform parent);
        Optional<CubeView> CreateCubeView(Transform parent, CubeConfig config);
        void DestroyCubeView(CubeView cubeView);
    }
}