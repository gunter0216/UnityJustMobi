using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config.Dto;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    public class JsonCubesConfigLoader : ICubesConfigLoader
    {
        private const string m_AssetKey = "CubesConfigJson";

        private readonly IConfigLoader m_ConfigLoader;

        public JsonCubesConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<CubesConfig> Load()
        {
            var dto = m_ConfigLoader.LoadConfig<CubesConfigDto>(m_AssetKey);
            if (!dto.HasValue)
            {
                Debug.LogError("[JsonCubesConfigLoader] In method Initialize, error load CoreConfig.");
                return Optional<CubesConfig>.Fail();
            }

            var cubes = new CubeConfig[dto.Value.Cubes.Count];
            for (int i = 0; i < dto.Value.Cubes.Count; i++)
            {
                var cubeDto = dto.Value.Cubes[i];
                cubes[i] = new CubeConfig(cubeDto.Key, cubeDto.AssetKey);
            }
            
            var config = new CubesConfig(cubes);

            return Optional<CubesConfig>.Success(config);
        }
    }
}