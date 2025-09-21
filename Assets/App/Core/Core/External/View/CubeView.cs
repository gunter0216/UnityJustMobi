using UnityEngine;
using UnityEngine.UI;

namespace App.Core.Core.External.View
{
    public class CubeView : MonoBehaviour
    {
        [SerializeField] private Image m_Icon;
        
        public void SetIcon(Sprite sprite)
        {
            m_Icon.sprite = sprite;
        }
    }
}