using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config.Scriptable;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    public class ScriptableCoreConfigLoader : ICoreConfigLoader
    {
        private const string m_AssetKey = "CoreConfig";

        private readonly IConfigLoader m_ConfigLoader;

        public ScriptableCoreConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<CoreConfig> Load()
        {
            var scriptable = m_ConfigLoader.LoadConfig<CoreConfigScriptableObject>(m_AssetKey);
            if (!scriptable.HasValue)
            {
                Debug.LogError("[CoreConfigLoader] In method Initialize, error load CoreConfig.");
                return Optional<CoreConfig>.Fail();
            }
            
            var config = new CoreConfig(scriptable.Value.Cubes);

            return Optional<CoreConfig>.Success(config);
        }
    }
}