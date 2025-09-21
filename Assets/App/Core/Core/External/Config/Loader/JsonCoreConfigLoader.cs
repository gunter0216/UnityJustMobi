using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Core.External.Config.Dto;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    public class JsonCoreConfigLoader : ICoreConfigLoader
    {
        private const string m_AssetKey = "CoreConfigJson";

        private readonly IConfigLoader m_ConfigLoader;

        public JsonCoreConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<CoreConfig> Load()
        {
            var dto = m_ConfigLoader.LoadConfig<CoreConfigDto>(m_AssetKey);
            if (!dto.HasValue)
            {
                Debug.LogError("[JsonCoreConfigLoader] In method Initialize, error load CoreConfig.");
                return Optional<CoreConfig>.Fail();
            }

            var cubes = new CubeConfig[dto.Value.Cubes.Count];
            for (int i = 0; i < dto.Value.Cubes.Count; i++)
            {
                var cubeDto = dto.Value.Cubes[i];
                cubes[i] = new CubeConfig(cubeDto.Key, cubeDto.AssetKey);
            }
            
            var config = new CoreConfig(cubes);

            return Optional<CoreConfig>.Success(config);
        }
    }
}