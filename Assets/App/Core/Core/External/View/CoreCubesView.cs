using UnityEngine;
using UnityEngine.UI;

namespace App.Core.Core.External.View
{
    public class CoreCubesView : MonoBehaviour
    {
        [SerializeField] private Transform m_CubesContent;
        [SerializeField] private CubeView m_CubeViewPrefab;
        [SerializeField] private ScrollRect m_ScrollRect;
        
        public Transform CubesContent => m_CubesContent;
        public ScrollRect ScrollRect => m_ScrollRect;
        public CubeView CubeViewPrefab => m_CubeViewPrefab;
    }
}