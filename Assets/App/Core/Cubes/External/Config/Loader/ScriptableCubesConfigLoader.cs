using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Cubes.External.Config.Scriptable;
using UnityEngine;

namespace App.Core.Cubes.External.Config.Loader
{
    public class ScriptableCubesConfigLoader : ICubesConfigLoader
    {
        private const string m_AssetKey = "CubesConfig";

        private readonly IConfigLoader m_ConfigLoader;

        public ScriptableCubesConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<CubesConfig> Load()
        {
            var scriptable = m_ConfigLoader.LoadConfig<CubesConfigScriptableObject>(m_AssetKey);
            if (!scriptable.HasValue)
            {
                Debug.LogError("[ScriptableCubesConfigLoader] In method Initialize, error load CoreConfig.");
                return Optional<CubesConfig>.Fail();
            }
            
            var config = new CubesConfig(scriptable.Value.Cubes);

            return Optional<CubesConfig>.Success(config);
        }
    }
}