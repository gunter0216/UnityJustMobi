using UnityEngine;
using UnityEngine.UI;

namespace App.Core.Core.External.View
{
    public class CubeView : MonoBehaviour 
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Icon;

        public RectTransform RectTransform => m_RectTransform;

        public void SetIcon(Sprite sprite)
        {
            m_Icon.sprite = sprite;
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public Vector3 GetGlobalPosition()
        {
            return transform.position;
        }

        public void SetGlobalPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}