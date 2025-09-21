using App.Common.AssetSystem.Runtime;
using App.Core.Core.External.Config;
using App.Core.Core.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Core.Core.External.Presenter
{
    public class CubePresenter
    {
        private readonly CubeView m_View;
        private readonly CubeConfig m_Config;
        private readonly ISpriteLoader m_SpriteLoader;

        public CubeConfig Config => m_Config;

        public CubePresenter(ISpriteLoader spriteLoader, CubeView view, CubeConfig config)
        {
            m_SpriteLoader = spriteLoader;
            m_View = view;
            m_Config = config;
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
        }
    }
}