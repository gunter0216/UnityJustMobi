using System;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Common.Configs.Runtime.ConfigConverters
{
    public class JsonConfigConverter : IConfigConverter
    {
        private readonly IJsonDeserializer m_JsonDeserializer;

        public JsonConfigConverter(IJsonDeserializer jsonDeserializer)
        {
            m_JsonDeserializer = jsonDeserializer;
        }

        public Optional<T> Convert<T>(Object obj) where T : class
        {
            if (obj is not TextAsset configJson)
            {
                Debug.LogError("[JsonConfigConverter] In method Convert, obj is not TextAsset.");
                return Optional<T>.Fail();
            }
            
            var config = m_JsonDeserializer.Deserialize<T>(configJson.text);
            return config;
        }

        public Type GetTargetType()
        {
            return typeof(TextAsset);
        }
    }
}