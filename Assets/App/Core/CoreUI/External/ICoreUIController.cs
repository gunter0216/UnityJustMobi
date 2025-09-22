using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using UnityEngine;

namespace App.Core.Core.External
{
    public interface ICoreUIController
    {
        Optional<CoreView> GetView();
        Optional<CubeView> CreateCubeView(Transform parent);
        Optional<CubeView> CreateCubeView(Transform parent, CubeConfig config);
        void DestroyCubeView(CubeView cubeView);
    }
}