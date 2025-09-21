using System.Collections.Generic;
using UnityEngine;

namespace App.Core.Core.External.Config
{
    [CreateAssetMenu(fileName = "CoreConfig", menuName = "Configs/CoreConfig")]
    public class CoreConfig : ScriptableObject
    {
        [SerializeField] private CubeConfig[] m_Cubes;
        
        public IReadOnlyList<CubeConfig> Cubes => m_Cubes;
    }
}