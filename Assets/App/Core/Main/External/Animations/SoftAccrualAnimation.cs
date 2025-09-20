using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Core.Main.External.Animations
{
    public class SoftAccrualAnimation : IDisposable, ISoftAccrualAnimation
    {
        private const string m_AssetKey = "SoftAccrualView";
        private readonly StringKeyEvaluator m_AssetKeyEvaluator = new(m_AssetKey);
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        private SoftAccrualView m_ViewPrefab;
        private ListPool<SoftAccrualView> m_Pool;

        public SoftAccrualAnimation(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public void Initialize()
        {
            var asset = m_AssetManager.LoadSync<GameObject>(m_AssetKeyEvaluator);
            if (!asset.HasValue)
            {
                Debug.LogError("[SoftAccrualAnimation] In method Initialize, Error in loading SoftAccrualView, check prefab");
                return;
            }
            
            m_ViewPrefab = asset.Value.GetComponent<SoftAccrualView>();
        
            m_Pool = new ListPool<SoftAccrualView>(
                CreateItem, 
                capacity: 10,
                getCallback: (item) => item.SetActive(true),
                releaseCallback: (item) => item.SetActive(false));
        }

        private Optional<SoftAccrualView> CreateItem()
        {
            var view = Object.Instantiate(m_ViewPrefab, m_Canvas.GetContent());
            if (view == null)
            {
                Debug.LogError("[SoftAccrualAnimation] In method CreateItem, Error in instantiate SoftAccrualView, check prefab");
                return Optional<SoftAccrualView>.Fail();
            }
            
            return Optional<SoftAccrualView>.Success(view);
        }

        public void PlayAnimation(Vector3 localPosition, Transform parent, long amount)
        {
            var view = m_Pool.Get();
            if (!view.HasValue)
            {
                Debug.LogError($"[SoftAccrualAnimation] In method PlayAnimation, class is not init.");
                return;
            }
            
            view.Value.SetParent(parent);
            view.Value.SetCountText($"+{amount.ToString()}");
            view.Value.SetLocalPosition(localPosition);
            view.Value.SetAsLastSibling();
            
            PlayAnimation(view.Value);
        }

        private void PlayAnimation(SoftAccrualView view, Action callback = null)
        {
            const float duration = 0.8f;
            
            var transform = view.transform;
            var endPosition = transform.localPosition.y + 100;
            
            DOTween.Sequence()
                .Append(transform.DOLocalMoveY(endPosition, duration))
                .OnComplete(() =>
                {
                    m_Pool.Release(view);
                    callback?.Invoke();
                });
        }

        public void Dispose()
        {
            m_Pool?.Dispose();
        }
    }
}