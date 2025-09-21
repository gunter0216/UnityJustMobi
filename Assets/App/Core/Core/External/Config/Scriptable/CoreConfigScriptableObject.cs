using System.Collections.Generic;
using UnityEngine;

namespace App.Core.Core.External.Config.Scriptable
{
    [CreateAssetMenu(fileName = "CoreConfig", menuName = "Configs/CoreConfig")]
    public class CoreConfigScriptableObject : ScriptableObject
    {
        [SerializeField] private CubeConfig[] m_Cubes;
        
        public IReadOnlyList<CubeConfig> Cubes => m_Cubes;
    }
}