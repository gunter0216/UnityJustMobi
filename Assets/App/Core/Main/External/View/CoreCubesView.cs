using UnityEngine;

namespace App.Core.Main.External.View
{
    public class CoreCubesView : MonoBehaviour
    {
        [SerializeField] private Transform m_CubesContent;
        [SerializeField] private CubeView m_CubeViewPrefab;
        
        public Transform CubesContent => m_CubesContent;
        public CubeView CubeViewPrefab => m_CubeViewPrefab;
    }
}