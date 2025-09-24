using System;
using App.Common.Events.External;
using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.CubeDragger.External.Animation;
using App.Core.Cubes.External.Config;
using App.Core.CubesPanel.External;
using App.Core.CubesPanel.External.Presenter;
using App.Core.Hole.External;
using App.Core.Tower.External;
using App.Core.Tower.External.Events;
using App.Core.Utility.External;
using App.Game.SpriteLoaders.Runtime;
using UniRx;
using UnityEngine;

namespace App.Core.CubeDragger.External
{
    /// <summary>
    /// Отвечает за перетаскивание кубов из панели и башни.
    /// При отпускании пытается закинуть в башню, потом в дыру и если не получается, уничтожает.
    /// </summary>
    public class DragCubeController : IInitSystem, IDisposable 
    {
        private const float m_LerpForce = 30f;
        
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ICoreUIController m_CoreUIController;
        private readonly IHoleController m_HoleController;
        private readonly ITowerController m_TowerController;
        private readonly IMessageController m_MessageController;
        private readonly IEventManager m_EventManager;

        private readonly CompositeDisposable m_Disposables = new();

        private CubeDisappearAnimation m_CubeDisappearAnimation;
        private CoreView m_CoreView;

        private CubeView m_View;
        private CubeConfig m_Config;

        private bool m_IsDragging = false;
        private Camera m_Camera;
        
        public DragCubeController(
            ISpriteLoader spriteLoader, 
            ICoreUIController coreUIController, 
            IHoleController holeController,
            ITowerController towerController, 
            IMessageController messageController, 
            IEventManager eventManager)
        {
            m_SpriteLoader = spriteLoader;
            m_CoreUIController = coreUIController;
            m_HoleController = holeController;
            m_TowerController = towerController;
            m_MessageController = messageController;
            m_EventManager = eventManager;
        }

        public void Init()
        {
            var view = m_CoreUIController.GetView();
            if (!view.HasValue)
            {
                return;
            }

            m_CoreView = view.Value;
            
            if (!CreateView())
            {
                return;
            }
            
            m_View.SetActive(false);
            m_Camera = Camera.main;

            m_CubeDisappearAnimation = new CubeDisappearAnimation(m_CoreUIController, m_CoreView);
            
            Observable.EveryUpdate().Subscribe(OnUpdate).AddTo(m_Disposables);
            m_EventManager.Subscribe<TemplateCubeStartDragEvent>(OnTemplateCubeStartDrag).AddTo(m_Disposables);
            m_EventManager.Subscribe<TowerCubeStartDragEvent>(OnTowerCubeStartDrag).AddTo(m_Disposables);
        }

        private void OnTowerCubeStartDrag(TowerCubeStartDragEvent dragEvent)
        {
            var presenter = dragEvent.Presenter;
            OnCubeStartDrag(presenter.TowerCube.Config, presenter.GetGlobalPosition());
        }

        private void OnTemplateCubeStartDrag(TemplateCubeStartDragEvent dragEvent)
        {
            var presenter = dragEvent.Presenter;
            OnCubeStartDrag(presenter.Config, presenter.GetGlobalPosition());
        }

        private void OnCubeStartDrag(CubeConfig config, Vector3 position)
        {
            if (m_IsDragging)
            {
                return;
            }
            
            m_Config = config;
            m_IsDragging = true;
            m_View.SetActive(true);
            m_View.SetGlobalPosition(position);

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
                var isUsed = m_TowerController.DropCubeOnTower(m_View, m_Config) != DropOnTowerStatus.NotIntersected;
                if (!isUsed)
                {
                    isUsed = m_HoleController.DropInHole(m_View, m_Config);
                } 

                if (!isUsed)
                {
                    m_CubeDisappearAnimation.Disappear(m_View, m_Config);
                    m_MessageController.ShowMessage("Disappear");
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
            var sprite = m_SpriteLoader.Load(m_Config.AssetKey);
            if (!sprite.HasValue)
            {
                Debug.LogError("Cant load sprite for cube");
                return;
            }

            m_View.SetIcon(sprite.Value);
        }

        private bool CreateView()
        {
            var view = m_CoreUIController.CreateCubeView(m_CoreView.ActiveCubeParent);
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