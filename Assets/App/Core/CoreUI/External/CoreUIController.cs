using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.CoreUI.External.Fabric;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External.Config;
using App.Core.CubesPanel.External.Presenter.Fabric;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.CoreUI.External
{
    /// <summary>
    /// Отвечает за создание основного UI (CoreView) и создание кубиков (CubeView), а так же их уничтожения.
    /// </summary>
    public class CoreUIController : IInitSystem, ICoreUIController
    {
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private CoreView m_View;
        private CubeViewCreator m_CubeViewCreator;
        private ListPool<CubeView> m_CubesPool;

        public CoreUIController(MainCanvas mainCanvas, IAssetManager assetManager, ISpriteLoader spriteLoader)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
            m_SpriteLoader = spriteLoader;
        }

        public void Init()
        {
            var creator = new CoreViewCreator(m_AssetManager, m_MainCanvas);
            var view = creator.Create();
            if (!view.HasValue)
            {
                Debug.LogError($"Cant create CoreView");
                return;
            }

            m_View = view.Value;
            
            m_CubeViewCreator = new CubeViewCreator(m_View.CubesView);
            
            m_CubesPool = new ListPool<CubeView>(
                () => m_CubeViewCreator.Create(m_MainCanvas.GetContent()), 
                30,
                getCallback: view => view.SetActive(true),
                releaseCallback: view => view.SetActive(false));
        }
        
        public Optional<CoreView> GetView()
        {
            if (m_View == null)
            {
                return Optional<CoreView>.Fail();
            }

            return Optional<CoreView>.Success(m_View);
        }

        public Optional<CubeView> CreateCubeView(Transform parent, CubeConfig config)
        {
            var view = CreateCubeView(parent);
            if (!view.HasValue)
            {
                Debug.LogError("Cant create CubeView");
                return Optional<CubeView>.Fail();
            }
            
            var sprite = m_SpriteLoader.Load(config.AssetKey);
            if (!sprite.HasValue)
            {
                Debug.LogError("Cant load sprite for cube");
                return Optional<CubeView>.Fail();
            }

            view.Value.SetIcon(sprite.Value);

            return Optional<CubeView>.Success(view.Value);
        }

        public Optional<CubeView> CreateCubeView(Transform parent)
        {
            if (m_View == null)
            {
                return Optional<CubeView>.Fail();
            }

            var view = m_CubesPool.Get();
            if (!view.HasValue)
            {
                Debug.LogError("Cant get CubeView from pool");
                return Optional<CubeView>.Fail();
            }
            
            view.Value.transform.localScale = Vector3.one;
            view.Value.transform.rotation = Quaternion.identity;
            view.Value.SetParent(parent);

            return Optional<CubeView>.Success(view.Value);
        }

        public void DestroyCubeView(CubeView cubeView)
        {
            m_CubesPool.Release(cubeView);
        }
    }
}