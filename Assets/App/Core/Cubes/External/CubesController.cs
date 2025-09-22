using System.Collections.Generic;
using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config;
using UnityEngine;

namespace App.Core.Core.External
{
    public class CubesController : IInitSystem, ICubesController
    {
        private readonly IConfigLoader m_ConfigLoader;
        
        private CubesConfigController m_ConfigController;

        public CubesController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public void Init()
        {
            // todo change loader here
            // var cubesConfigLoader = new JsonCubesConfigLoader(m_ConfigLoader);
            var cubesConfigLoader = new ScriptableCubesConfigLoader(m_ConfigLoader);
            
            m_ConfigController = new CubesConfigController(cubesConfigLoader);
            if (!m_ConfigController.Initialize())
            {
                Debug.LogError($"Cant initialize CoreConfigController");
                return;
            }
        }
        
        public IReadOnlyList<CubeConfig> GetCubes()
        {
            return m_ConfigController.GetCubes();
        }
        
        public Optional<CubeConfig> GetCubeConfig(string key)
        {
            return m_ConfigController.GetCubeConfig(key);
        }
    }
}