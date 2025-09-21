using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Core.External.View;

namespace App.Core.Core.External.Presenter.Fabric
{
    public class CoreViewCreator
    {
        private const string m_AssetKey = "CoreView";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        public CoreViewCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public Optional<CoreView> Create()
        {
            var view = m_AssetManager.InstantiateSync<CoreView>(
                new StringKeyEvaluator(m_AssetKey), 
                m_Canvas.GetContent());
            if (!view.HasValue)
            {
                return Optional<CoreView>.Fail();
            }
            
            return view;
        }
    }
}