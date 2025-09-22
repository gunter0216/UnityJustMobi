using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Cubes.External.Config;

namespace App.Core.Cubes.External
{
    public interface ICubesController
    {
        IReadOnlyList<CubeConfig> GetCubes();
        Optional<CubeConfig> GetCubeConfig(string key);
    }
}