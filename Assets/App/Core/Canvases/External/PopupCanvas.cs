using UnityEngine;

namespace App.Core.Canvases.External
{
    public class PopupCanvas : MonoBehaviour, ICanvas
    {
        [SerializeField] private Transform m_Content;
        
        public Transform GetContent()
        {
            return m_Content;
        }
    }
}