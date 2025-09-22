using App.Common.Utilities.Utility.Runtime;

namespace App.Core.Cubes.External.Config.Loader
{
    public interface ICubesConfigLoader
    {
        Optional<CubesConfig> Load();
    }
}