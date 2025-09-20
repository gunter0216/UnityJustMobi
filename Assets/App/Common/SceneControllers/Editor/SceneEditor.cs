﻿using App.Common.SceneControllers.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Common.SceneControllers.Editor
{
    public class SceneEditor
    {
#if UNITY_EDITOR
        [MenuItem("Helper/Scenes/Start", false, 1)]
        public static void GoToStartScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.StartScene}.unity");
        }
        
        [MenuItem("Helper/Scenes/Core", false, 2)]
        public static void GoToCoreScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.CoreScene}.unity");
        }

        private static void OpenScene(string name)
        {
            if (Application.isPlaying)
            {
                Debug.Log("Open scene only in Edit mode!");
                return;
            }
            
            EditorSceneManager.OpenScene(name);
        }
#endif
    }
}