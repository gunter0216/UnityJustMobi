using UnityEngine;

namespace App.Core.CoreUI.External.View
{
    public class CoreView : MonoBehaviour
    {
        [SerializeField] private CoreCubesView m_CubesView;
        [SerializeField] private Transform m_ActiveCubeParent;
        [SerializeField] private Transform m_DisappearCubeParent;
        [SerializeField] private TowerView m_TowerView;
        [SerializeField] private HoleView m_HoleView;

        public CoreCubesView CubesView => m_CubesView;
        public Transform ActiveCubeParent => m_ActiveCubeParent;
        public TowerView TowerView => m_TowerView;
        public HoleView HoleView => m_HoleView;
        public Transform DisappearCubeParent => m_DisappearCubeParent;
    }
}