using System;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Common.Configs.Runtime.ConfigConverters
{
    public class ScriptableObjectConfigConverter : IConfigConverter
    {
        public Optional<T> Convert<T>(Object obj) where T : class
        {
            if (obj is not T config)
            {
                Debug.LogError($"[ScriptableObjectConfigConverter] In method Convert, obj is not {typeof(T).Name}.");
                return Optional<T>.Fail();
            }

            return Optional<T>.Success(config);
        }

        public Type GetTargetType()
        {
            return typeof(ScriptableObject); 
        }
    }
}