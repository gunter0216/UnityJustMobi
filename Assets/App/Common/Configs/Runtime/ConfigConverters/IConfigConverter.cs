using System;
using App.Common.Utilities.Utility.Runtime;
using Object = UnityEngine.Object;

namespace App.Common.Configs.Runtime.ConfigConverters
{
    public interface IConfigConverter
    {
        Optional<T> Convert<T>(Object obj) where T : class;
        Type GetTargetType();
    }
}