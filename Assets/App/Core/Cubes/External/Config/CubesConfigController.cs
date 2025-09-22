using System.Collections.Generic;
using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    public class CubesConfigController
    {
        private readonly ICubesConfigLoader m_CubesConfigLoader;

        private Dictionary<string, CubeConfig> m_Cubes;
        private CubesConfig m_Config;

        public CubesConfigController(ICubesConfigLoader cubesConfigLoader)
        {
            m_CubesConfigLoader = cubesConfigLoader;
        }   

        public bool Initialize()
        {
            var config = m_CubesConfigLoader.Load();
            if (!config.HasValue)
            {
                Debug.LogError("[CubesConfigController] In method Initialize, error load CoreConfig.");
                return false;
            }

            m_Config = config.Value;

            m_Cubes = new Dictionary<string, CubeConfig>(m_Config.Cubes.Count);
            foreach (var cube in m_Config.Cubes)
            {
                m_Cubes[cube.Key] = cube;
            }

            return true;
        }
     
        public IReadOnlyList<CubeConfig> GetCubes()
        {
            return m_Config.Cubes;
        }
        
        public Optional<CubeConfig> GetCubeConfig(string key)
        {
            if (m_Cubes.TryGetValue(key, out var config))
            {
                return Optional<CubeConfig>.Success(config);
            }
            
            return Optional<CubeConfig>.Fail();
        }
    }
}