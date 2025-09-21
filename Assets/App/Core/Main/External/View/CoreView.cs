using UnityEngine;

namespace App.Core.Main.External.View
{
    public class CoreView : MonoBehaviour
    {
        [SerializeField] private CoreCubesView m_CubesView;

        public CoreCubesView CubesView => m_CubesView;
    }
}