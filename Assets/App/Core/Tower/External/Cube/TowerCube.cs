using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Data;

namespace App.Core.Tower.External
{
    public class TowerCube
    {
        private readonly CubeData m_Data;
        private readonly CubeConfig m_Config;

        public CubeData Data => m_Data;
        public CubeConfig Config => m_Config;
        
        public TowerCube(CubeData data, CubeConfig config)
        {
            m_Data = data;
            m_Config = config;
        }
    }
}