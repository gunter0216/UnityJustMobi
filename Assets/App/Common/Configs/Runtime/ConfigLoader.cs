using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime.ConfigConverters;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Configs.Runtime
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly IAssetManager m_AssetManager;
        private readonly IJsonDeserializer m_JsonDeserializer;

        private readonly AddressablesConfigLoader m_AddressablesConfigLoader;

        public ConfigLoader(IAssetManager assetManager, IJsonDeserializer jsonDeserializer)
        {
            m_AssetManager = assetManager;
            m_JsonDeserializer = jsonDeserializer;

            var configConverters = new List<IConfigConverter>()
            {
                new JsonConfigConverter(m_JsonDeserializer),
                new ScriptableObjectConfigConverter()
            };
            
            m_AddressablesConfigLoader = new AddressablesConfigLoader(m_AssetManager, configConverters);
        }
        
        public Optional<T> LoadConfig<T>(string key) where T : class
        {
            var config = m_AddressablesConfigLoader.Load<T>(key);
            return config;
        }
    }
}