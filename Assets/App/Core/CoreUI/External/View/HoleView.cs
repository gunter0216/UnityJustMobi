using UnityEngine;

namespace App.Core.Core.External.View
{
    public class HoleView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Transform m_MaskTransform;

        public RectTransform RectTransform => m_RectTransform;
        public Transform MaskTransform => m_MaskTransform;
    }
}