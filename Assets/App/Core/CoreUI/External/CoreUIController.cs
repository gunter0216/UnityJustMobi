using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Core.External.Presenter.Fabric;
using App.Core.Core.External.View;
using UnityEngine;

namespace App.Core.Core.External
{
    public class CoreUIController : IInitSystem, ICoreUIController
    {
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        
        private CoreView m_View;

        public CoreUIController(MainCanvas mainCanvas, IAssetManager assetManager)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
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
        }
        
        public Optional<CoreView> GetView()
        {
            if (m_View == null)
            {
                return Optional<CoreView>.Fail();
            }

            return Optional<CoreView>.Success(m_View);
        }
    }
}