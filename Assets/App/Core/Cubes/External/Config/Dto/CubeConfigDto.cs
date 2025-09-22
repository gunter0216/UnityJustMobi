using System;
using Newtonsoft.Json;

namespace App.Core.Cubes.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class CubeConfigDto
    {
        [JsonProperty("key")] private string m_Key;
        [JsonProperty("asset_key")] private string m_AssetKey;
        
        public string Key => m_Key;
        public string AssetKey => m_AssetKey;
    }
}