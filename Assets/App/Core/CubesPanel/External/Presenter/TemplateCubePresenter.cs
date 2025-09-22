using System;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Core.Core.External.Presenter
{
    public class TemplateCubePresenter
    {
        private const float m_AngleForDrag = 60f;

        private readonly CubeView m_View;
        private readonly CubeConfig m_Config;
        private readonly ScrollRect m_ScrollRect;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly Action<TemplateCubePresenter> m_OnStartDrag;

        private CubeDragHandler m_DragHandler;
        private bool m_IsMyDrag;

        public CubeConfig Config => m_Config;

        public TemplateCubePresenter(
            ISpriteLoader spriteLoader, 
            CubeView view, 
            CubeConfig config, 
            ScrollRect scrollRect,
            Action<TemplateCubePresenter> onStartDrag)
        {
            m_SpriteLoader = spriteLoader;
            m_View = view;
            m_Config = config;
            m_ScrollRect = scrollRect;
            m_OnStartDrag = onStartDrag;
        }

        public void Initialize()
        {
            var sprite = m_SpriteLoader.Load(Config.AssetKey);
            if (!sprite.HasValue)
            {
                Debug.LogError("Cant load sprite for cube");
                return;
            }

            m_View.SetIcon(sprite.Value);
            m_DragHandler = m_View.AddComponent<CubeDragHandler>();
            m_DragHandler.SetDragCallback(OnDrag);
            m_DragHandler.SetBeginDragCallback(OnBeginDrag);
            m_DragHandler.SetEndDragCallback(OnEndDrag);
        }

        private void OnBeginDrag(PointerEventData eventData)
        {
            float angle = Vector2.SignedAngle(Vector2.up, eventData.delta);
            
            if (Mathf.Abs(angle) <= m_AngleForDrag)
            {
                m_IsMyDrag = true;
                m_OnStartDrag?.Invoke(this);
            }
            else
            {
                m_IsMyDrag = false;
                m_ScrollRect.OnBeginDrag(eventData);
            }
        }

        private void OnDrag(PointerEventData eventData)
        {
            if (m_IsMyDrag)
            {
            }
            else
            {
                m_ScrollRect.OnDrag(eventData);
            }
        }

        private void OnEndDrag(PointerEventData eventData)
        {
            if (m_IsMyDrag)
            {
            }
            else
            {
                m_ScrollRect.OnEndDrag(eventData);
            }
            
            m_IsMyDrag = false; 
        }
        
        public Vector3 GetGlobalPosition()
        {
            return m_View.GetGlobalPosition();
        }
    }
}