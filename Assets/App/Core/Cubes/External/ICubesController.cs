using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;

namespace App.Core.Core.External
{
    public interface ICubesController
    {
        IReadOnlyList<CubeConfig> GetCubes();
        Optional<CubeConfig> GetCubeConfig(string key);
    }
}