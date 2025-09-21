using System.Collections.Generic;
using System.Linq;
using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime.ConfigConverters;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Configs.Runtime
{
    public class AddressablesConfigLoader
    {
        private readonly IAssetManager m_AssetManager;
        private readonly IReadOnlyList<IConfigConverter> m_ConfigConverters;

        public AddressablesConfigLoader(IAssetManager assetManager, IReadOnlyList<IConfigConverter> configConverters)
        {
            m_AssetManager = assetManager;
            m_ConfigConverters = configConverters;
        }

        public Optional<T> Load<T>(string localKey) where T : class
        {
            var keyEvaluator = new StringKeyEvaluator(localKey);
            var config = m_AssetManager.LoadSync<Object>(keyEvaluator);
            if (!config.HasValue)
            {
                Debug.LogError($"[AddressablesConfigLoader] In method Load, config is not preloaded {typeof(T).Name} with key {localKey}.");
                return new Optional<T>(default, false);
            }

            var configObj = config.Value;
            var configConverter = m_ConfigConverters.FirstOrDefault(converter =>
                converter.GetTargetType().IsInstanceOfType(configObj));
            if (configConverter != null)
            {
                var convertedConfig = configConverter.Convert<T>(configObj);
                m_AssetManager.UnloadAsset(keyEvaluator);
                return convertedConfig;
            }
            
            Debug.LogError($"Not found config converter for type {configObj.GetType().Name} with key {localKey}.");
            
            m_AssetManager.UnloadAsset(keyEvaluator);
            
            return Optional<T>.Fail();
        }
    }
}