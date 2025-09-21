using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Configs.Runtime
{
    public interface IConfigLoader
    {
        Optional<T> LoadConfig<T>(string key) where T : class;
    }
}