using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Core.StartScene.External
{
    /// <summary>
    /// Отвечает за загрузку основной сцены из стартовой
    /// </summary>
    public class StartSceneController  : IInitSystem
    {
        private readonly ISceneManager m_SceneManager;

        public StartSceneController(ISceneManager sceneManager)
        {
            m_SceneManager = sceneManager;
        }

        public void Init()
        {
            m_SceneManager.LoadScene(SceneConstants.CoreScene);
        }
    }
}