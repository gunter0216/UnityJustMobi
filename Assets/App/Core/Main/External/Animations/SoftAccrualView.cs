using TMPro;
using UnityEngine;

namespace App.Core.Main.External.Animations
{
    public class SoftAccrualView : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_Count;

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }
        
        public void SetCountText(string text)
        {
            m_Count.text = text;
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