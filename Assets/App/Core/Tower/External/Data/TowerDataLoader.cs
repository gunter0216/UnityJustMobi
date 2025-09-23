using App.Common.Data.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Core.Tower.External.Data
{
    public class TowerDataLoader
    {
        private readonly IDataManager m_DataManager;

        public TowerDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public Optional<TowerData> Load()
        {
            var data = m_DataManager.GetData<TowerData>(TowerData.Key);
            if (!data.HasValue)
            {
                Debug.LogError("TowerData not found");
            }

            return data;
        }
    }
}