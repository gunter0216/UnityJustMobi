using UnityEngine;

namespace App.Core.Core.External.View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;

        public RectTransform RectTransform => m_RectTransform;
    }
}