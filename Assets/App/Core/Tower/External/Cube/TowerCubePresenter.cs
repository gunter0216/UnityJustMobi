using System;
using App.Core.CoreUI.External.View;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Core.Tower.External
{
    public class TowerCubePresenter
    {
        private readonly TowerCube m_TowerCube;
        private readonly CubeView m_View;
        
        private readonly Action<TowerCubePresenter> m_OnStartDrag;

        private CubeDragHandler m_DragHandler;
        
        public TowerCube TowerCube => m_TowerCube;
        public CubeView View => m_View;

        public TowerCubePresenter(TowerCube towerCube, CubeView view, Action<TowerCubePresenter> onStartDrag)
        {
            m_TowerCube = towerCube;
            m_View = view;
            m_OnStartDrag = onStartDrag;
        }

        public void Initialize()
        {
            m_DragHandler = m_View.GetComponent<CubeDragHandler>();
            if (m_DragHandler == null)
            {
                m_DragHandler = m_View.AddComponent<CubeDragHandler>();
            }
            
            m_DragHandler.SetBeginDragCallback(OnBeginDrag);
        }

        private void OnBeginDrag(PointerEventData eventData)
        {
            m_OnStartDrag?.Invoke(this);
        }
        
        public Vector3 GetGlobalPosition()
        {
            return m_View.GetGlobalPosition();
        }
    }
}