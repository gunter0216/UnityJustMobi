using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Core.Tower.External.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TowerData : IData
    {
        public const string Key = nameof(TowerData);
        
        [JsonProperty("cubes")] private List<CubeData> m_Cubes;

        public List<CubeData> Cubes
        {
            get => m_Cubes;
            set => m_Cubes = value;
        }

        public string Name()
        {
            return Key;
        }
    }
}