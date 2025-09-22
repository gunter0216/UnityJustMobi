using System.Collections.Generic;

namespace App.Core.Core.External.Config
{
    public class CubesConfig
    {
        private readonly IReadOnlyList<CubeConfig> m_Cubes;

        public CubesConfig(IReadOnlyList<CubeConfig> cubes)
        {
            m_Cubes = cubes;
        }

        public IReadOnlyList<CubeConfig> Cubes => m_Cubes;
    }
}