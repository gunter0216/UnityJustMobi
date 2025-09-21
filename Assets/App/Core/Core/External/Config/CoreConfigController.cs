using System.Collections.Generic;
using App.Common.Configs.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    public class CoreConfigController
    {
        private readonly IConfigLoader m_ConfigLoader;
        private readonly ICoreConfigLoader m_CoreConfigLoader;

        private CoreConfig m_Config;

        public CoreConfigController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
            
            // todo change loader here
            // m_CoreConfigLoader = new ScriptableCoreConfigLoader(m_ConfigLoader);
            m_CoreConfigLoader = new JsonCoreConfigLoader(m_ConfigLoader);
        }   

        public bool Initialize()
        {
            var config = m_CoreConfigLoader.Load();
            if (!config.HasValue)
            {
                Debug.LogError("[CoreConfigController] In method Initialize, error load CoreConfig.");
                return false;
            }

            m_Config = config.Value;

            return true;
        }
     
        public IReadOnlyList<CubeConfig> GetCubes()
        {
            return m_Config.Cubes;
        }
    }
}