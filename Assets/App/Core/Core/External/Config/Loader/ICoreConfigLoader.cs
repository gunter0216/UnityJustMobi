using App.Common.Utilities.Utility.Runtime;

namespace App.Core.Core.External.Config
{
    public interface ICoreConfigLoader
    {
        Optional<CoreConfig> Load();
    }
}