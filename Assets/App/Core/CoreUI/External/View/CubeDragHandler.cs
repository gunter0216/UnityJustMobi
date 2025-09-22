using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Core.CoreUI.External.View
{
    public class CubeDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
    {
        private Action<PointerEventData> m_DragCallback;
        private Action<PointerEventData> m_BeginDragCallback;
        private Action<PointerEventData> m_EndDragCallback;

        public void SetDragCallback(Action<PointerEventData> callback)
        {
            m_DragCallback = callback;
        }
        
        public void SetBeginDragCallback(Action<PointerEventData> callback)
        {
            m_BeginDragCallback = callback;
        }
        
        public void SetEndDragCallback(Action<PointerEventData> callback)
        {
            m_EndDragCallback = callback;
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_DragCallback?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_BeginDragCallback?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_EndDragCallback?.Invoke(eventData);
        }
    }
}