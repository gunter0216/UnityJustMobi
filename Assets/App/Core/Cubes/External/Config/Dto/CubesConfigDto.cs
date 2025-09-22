using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Core.Core.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class CubesConfigDto
    {
        [JsonProperty("cubes")] private CubeConfigDto[] m_Cubes;
        
        public IReadOnlyList<CubeConfigDto> Cubes => m_Cubes;
    }
}