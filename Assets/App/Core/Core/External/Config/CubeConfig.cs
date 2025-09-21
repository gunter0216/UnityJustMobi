using System;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    [Serializable]
    public class CubeConfig
    {
        [SerializeField] private string m_Key;
        [SerializeField] private string m_AssetKey;

        public string Key => m_Key;
        public string AssetKey => m_AssetKey;
        
        public CubeConfig()
        {
        }
        
        public CubeConfig(string key, string assetKey)
        {
            m_Key = key;
            m_AssetKey = assetKey;
        }
    }
}