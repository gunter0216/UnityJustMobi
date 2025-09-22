using TMPro;
using UnityEngine;

namespace App.Core.Menu.External.Animations
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private TMP_Text m_Text;

        public RectTransform RectTransform => m_RectTransform;

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }
        
        public void SetText(string text)
        {
            m_Text.text = text;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetGlobalPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetLocalPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }

        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }
    }
}