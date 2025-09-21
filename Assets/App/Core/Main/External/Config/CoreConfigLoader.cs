using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Core.Main.External.Config
{
    public class CoreConfigLoader
    {
        private const string m_AssetKey = "CoreConfig";

        private readonly IConfigLoader m_ConfigLoader;

        public CoreConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<CoreConfig> Load()
        {
            var config = m_ConfigLoader.LoadConfig<CoreConfig>(m_AssetKey);
            if (!config.HasValue)
            {
                Debug.LogError("[CoreConfigLoader] In method Initialize, error load CoreConfig.");
                return Optional<CoreConfig>.Fail();
            }

            return Optional<CoreConfig>.Success(config.Value);
        }
    }
}