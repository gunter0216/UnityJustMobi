using System.Collections.Generic;

namespace App.Core.Core.External.Config
{
    public class CoreConfig
    {
        private readonly IReadOnlyList<CubeConfig> m_Cubes;

        public CoreConfig(IReadOnlyList<CubeConfig> cubes)
        {
            m_Cubes = cubes;
        }

        public IReadOnlyList<CubeConfig> Cubes => m_Cubes;
    }
}