using System.Collections.Generic;
using UnityEngine;

namespace App.Core.Core.External.Config.Scriptable
{
    [CreateAssetMenu(fileName = "CubesConfig", menuName = "Configs/CubesConfig")]
    public class CubesConfigScriptableObject : ScriptableObject
    {
        [SerializeField] private CubeConfig[] m_Cubes;
        
        public IReadOnlyList<CubeConfig> Cubes => m_Cubes;
    }
}