using UnityEngine;

namespace App.Core.Main.External.Config
{
    [CreateAssetMenu(fileName = "CoreConfig", menuName = "Configs/CoreConfig")]
    public class CoreConfig : ScriptableObject
    {
        [SerializeField] private long m_MaxBreedsCount;
        
        public long MaxBreedsCount => m_MaxBreedsCount;
    }
}