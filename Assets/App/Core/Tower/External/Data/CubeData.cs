using System;
using Newtonsoft.Json;

namespace App.Core.Tower.External.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class CubeData
    {
        [JsonProperty("key")] private string m_Key;
        [JsonProperty("x")] private float m_PositionX;
        [JsonProperty("y")] private float m_PositionY;

        public string Key
        {
            get => m_Key;
            set => m_Key = value;
        }

        public float PositionX
        {
            get => m_PositionX;
            set => m_PositionX = value;
        }

        public float PositionY
        {
            get => m_PositionY;
            set => m_PositionY = value;
        }
    }
}