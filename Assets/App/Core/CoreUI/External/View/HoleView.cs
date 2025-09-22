using UnityEngine;

namespace App.Core.CoreUI.External.View
{
    public class HoleView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Transform m_MaskTransform;
        [SerializeField] private Transform m_Top;
        [SerializeField] private Transform m_Bottom;

        public RectTransform RectTransform => m_RectTransform;
        public Transform MaskTransform => m_MaskTransform;
        public Transform Top => m_Top;
        public Transform Bottom => m_Bottom;
    }
}