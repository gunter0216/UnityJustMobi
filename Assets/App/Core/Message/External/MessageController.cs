using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Menu.External.Animations;

namespace App.Core.CubeDragger.External
{
    /// <summary>
    /// Отвечает за показ сообщений на экране
    /// </summary>
    public class MessageController : IInitSystem, IMessageController
    {
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        
        private MessageAnimation m_MessageAnimation;
        
        public MessageController(MainCanvas mainCanvas, IAssetManager assetManager)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
        }

        public void Init()
        {
            m_MessageAnimation = new MessageAnimation(m_AssetManager, m_MainCanvas);
            m_MessageAnimation.Initialize();
        }

        public void ShowMessage(string message)
        {
            m_MessageAnimation.PlayAnimation(message);
        }
    }
}