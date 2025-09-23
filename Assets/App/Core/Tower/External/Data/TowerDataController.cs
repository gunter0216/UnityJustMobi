using System.Collections.Generic;
using App.Common.Data.Runtime;

namespace App.Core.Tower.External.Data
{
    public class TowerDataController
    {
        private readonly IDataManager m_DataManager;
        
        private TowerData m_Data;

        public TowerDataController(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public bool Initialize()
        {
            var dataLoader = new TowerDataLoader(m_DataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                return false;
            }

            m_Data = data.Value;
            
            m_Data.Cubes ??= new List<CubeData>();
            
            return true;
        }
        
        public IReadOnlyList<CubeData> GetCubes()
        {
            return m_Data.Cubes;
        }
        
        public void AddCube(CubeData cubeData)
        {
            m_Data.Cubes.Add(cubeData);
        }
        
        public void RemoveCube(CubeData cubeData)
        {
            m_Data.Cubes.Remove(cubeData);
        }
    }
}