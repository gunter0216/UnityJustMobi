using App.Common.Utilities.Utility.Runtime;

namespace App.Core.Core.External.Config
{
    public interface ICubesConfigLoader
    {
        Optional<CubesConfig> Load();
    }
}