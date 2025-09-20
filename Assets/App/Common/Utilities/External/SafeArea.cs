using UnityEngine;

namespace App.Common.Utilities.External
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform rectTransform;
    
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }
    
        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
        
            Vector2 anchorMin = rectTransform.anchorMin;
            Vector2 anchorMax = rectTransform.anchorMax;
        
            anchorMin.y = safeArea.y / Screen.height;
            anchorMax.y = (safeArea.y + safeArea.height) / Screen.height;
        
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}