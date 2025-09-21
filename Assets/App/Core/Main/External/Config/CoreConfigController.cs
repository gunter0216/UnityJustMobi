using App.Common.Configs.Runtime;
using UnityEngine;

namespace App.Core.Main.External.Config
{
    public class CoreConfigController
    {
        private readonly IConfigLoader m_ConfigLoader;

        private CoreConfig m_Config;

        public CoreConfigController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public bool Initialize()
        {
            var configLoader = new CoreConfigLoader(m_ConfigLoader);
            var config = configLoader.Load();
            if (!config.HasValue)
            {
                Debug.LogError("[CoreConfigController] In method Initialize, error load CoreConfig.");
                return false;
            }

            m_Config = config.Value;

            return true;
        }
     
        public long GetMaxBreedsCount()
        {
            return m_Config.MaxBreedsCount < 0 ? 10 : m_Config.MaxBreedsCount;
        }
    }
    
    
}