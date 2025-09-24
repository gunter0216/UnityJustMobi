using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.External;
using App.Core.CoreUI.External;
using App.Core.CoreUI.External.View;
using App.Core.Cubes.External;
using App.Core.Cubes.External.Config;
using App.Core.Tower.External.Animations;
using App.Core.Tower.External.Cube;
using App.Core.Tower.External.Data;
using UnityEngine;

namespace App.Core.Tower.External.Presenter
{
    public class TowerPresenter
    {
        private readonly TowerDataController m_DataController;
        private readonly ICubesController m_CubesController;
        private readonly ICoreUIController m_CoreUIController;
        private readonly TowerView m_TowerView;
        
        private event Action<TowerCubePresenter> m_OnTowerCubeStartDrag;

        private List<TowerCubePresenter> m_CubePresenters;
        private TowerFallCubesAnimation m_TowerFallCubesAnimation;

        public TowerPresenter(
            TowerDataController dataController, 
            TowerView towerView, 
            ICubesController cubesController, 
            ICoreUIController coreUIController, 
            Action<TowerCubePresenter> onTowerCubeStartDrag)
        {
            m_DataController = dataController;
            m_TowerView = towerView;
            m_CubesController = cubesController;
            m_CoreUIController = coreUIController;
            m_OnTowerCubeStartDrag = onTowerCubeStartDrag;
        }

        public void Initialize()
        {
            m_CubePresenters = new List<TowerCubePresenter>();
            m_TowerFallCubesAnimation = new TowerFallCubesAnimation();
            
            LoadTowerFromData();
        }
        
        public IReadOnlyList<TowerCubePresenter> GetCubes()
        {
            return m_CubePresenters;
        }
        
        public TowerCubePresenter GetLastCube()
        {
            return m_CubePresenters.Last();
        }
        
        private void LoadTowerFromData()
        {
            foreach (var cubeData in m_DataController.GetCubes())
            {
                var config = m_CubesController.GetCubeConfig(cubeData.Key);
                if (!config.HasValue)
                {
                    Debug.LogError($"[PlaceCubeStrategy] In method LoadTowerFromData, not found CubeConfig by key {cubeData.Key}.");
                    continue;
                }
                
                var cubeView = m_CoreUIController.CreateCubeView(m_TowerView.RectTransform, config.Value);
                if (!cubeView.HasValue)
                {
                    Debug.LogError("[PlaceCubeStrategy] In method PlaceCube, error create CubeView.");
                    return;
                }
                
                var presenter = CreateCubePresenterInternal(cubeData, config.Value, cubeView.Value);
                m_CubePresenters.Add(presenter);
            }
            
            GlobalCoroutineProvider.DoCoroutine(UpdateCubesPosition());
        }
        
        private IEnumerator UpdateCubesPosition()
        {
            yield return new WaitForEndOfFrame();
            foreach (var cubePresenter in m_CubePresenters)
            {
                var data = cubePresenter.TowerCube.Data;
                cubePresenter.View.SetGlobalPosition(new Vector3(data.PositionX, data.PositionY, 0));
            }
        }

        public TowerCubePresenter CreateCubePresenter(CubeData data, CubeConfig config, CubeView cubeView)
        {
            var presenter = CreateCubePresenterInternal(data, config, cubeView);
            
            m_CubePresenters.Add(presenter);
            m_DataController.AddCube(data);
            
            return presenter;
        }

        private TowerCubePresenter CreateCubePresenterInternal(CubeData data, CubeConfig config, CubeView cubeView)
        {
            var towerCube = new TowerCube(data, config);
            var presenter = new TowerCubePresenter(towerCube, cubeView, OnTowerCubeStartDrag);
            presenter.Initialize();
            
            return presenter;
        }
        
        private void OnTowerCubeStartDrag(TowerCubePresenter presenter)
        {
            var presenterIndex = m_CubePresenters.IndexOf(presenter);
            if (presenterIndex < 0)
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, not found TowerCubePresenter in list.");
                return;
            }
            
            m_OnTowerCubeStartDrag?.Invoke(presenter);

            PlayFallAnimation(presenterIndex);
            
            m_CoreUIController.DestroyCubeView(presenter.View);
            if (!m_CubePresenters.Remove(presenter))
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove TowerCubePresenter from list.");
            }

            if (!m_DataController.RemoveCube(presenter.TowerCube.Data))
            {
                Debug.LogError("[PlaceCubeStrategy] In method OnTowerCubeStartDrag, error remove CubeData from TowerDataController.");
            }
            
            presenter.Destroy();
        }

        private void PlayFallAnimation(int startIndex)
        {
            m_TowerFallCubesAnimation.PlayFallAnimation(startIndex, m_CubePresenters);
        }
    }
}