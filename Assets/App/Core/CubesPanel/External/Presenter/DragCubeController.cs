using System;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Core.Core.External.Config;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using App.Core.Utility.External;
using App.Game.SpriteLoaders.Runtime;
using UniRx;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class DragCubeController : IDisposable
    {
        private const float m_LerpForce = 30f;
        
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly CubeViewCreator m_CubeViewCreator;
        private readonly CoreView m_CoreView;

        private readonly CompositeDisposable m_Disposables = new();
        
        private CubeView m_View;
        private CubeConfig m_Config;

        private bool m_IsDragging = false;
        private Camera m_Camera;

        public CubeConfig Config => m_Config;
        
        public DragCubeController(CubeViewCreator cubeViewCreator, ISpriteLoader spriteLoader, CoreView coreView)
        {
            m_CubeViewCreator = cubeViewCreator;
            m_SpriteLoader = spriteLoader;
            m_CoreView = coreView;
        }

        public void Initialize()
        {
            if (!CreateView())
            {
                return;
            }
            
            m_View.SetActive(false);
            m_Camera = Camera.main;
            
            Observable.EveryUpdate().Subscribe(OnUpdate).AddTo(m_Disposables);
            
            // m_DragHandler = m_View.AddComponent<CubeDragHandler>();
            // m_DragHandler.SetDragCallback(OnDrag);
            // m_DragHandler.SetBeginDragCallback(OnBeginDrag);
            // m_DragHandler.SetEndDragCallback(OnEndDrag);
        }

        public void OnCubeStartDrag(TemplateCubePresenter templateCubePresenter)
        {
            if (m_IsDragging)
            {
                return;
            }
            
            m_Config = templateCubePresenter.Config;
            m_IsDragging = true;
            m_View.SetActive(true);
            m_View.SetGlobalPosition(templateCubePresenter.GetGlobalPosition());

            SetSprite();
        }

        private void OnUpdate(long _)
        {
            if (!m_IsDragging)
            {
                return;
            }
            
            FollowToMouse();
            
            if (Input.GetMouseButtonUp(0))
            {
                if (RectBoundsChecker.IsRectCompletelyInside(m_View.RectTransform, m_CoreView.TowerView.RectTransform))
                {
                    Debug.LogError("Drop cube into tower");
                }
                
                m_View.SetActive(false);
                m_IsDragging = false;
            }
        }

        private void FollowToMouse()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            mousePosition = m_Camera.ScreenToWorldPoint(mousePosition);
            
            var followedImagePosition = m_View.transform.position;
            var dragPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
            m_View.transform.position = Vector3.Lerp(followedImagePosition, dragPosition,
                Time.deltaTime * m_LerpForce);
        }

        private void SetSprite()
        {
            var sprite = m_SpriteLoader.Load(Config.AssetKey);
            if (!sprite.HasValue)
            {
                Debug.LogError("Cant load sprite for cube");
                return;
            }

            m_View.SetIcon(sprite.Value);
        }

        private bool CreateView()
        {
            var view = m_CubeViewCreator.Create(m_CoreView.ActiveCubeParent);
            if (!view.HasValue)
            {
                Debug.LogError("Cant create cube view");
                return false;
            }
            
            m_View = view.Value;
            return true;
        }

        public void Dispose()
        {
            m_Disposables?.Dispose();
        }
    }
}