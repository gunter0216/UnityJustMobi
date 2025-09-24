using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Localization.External;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Canvases.External;
using App.Core.Message.External.View;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Core.Message.External.Animation
{
    public class MessageAnimation : IDisposable
    {
        private const float m_StartPositionY = 300f;
        private const string m_AssetKey = "MessageView";
        private readonly StringKeyEvaluator m_AssetKeyEvaluator = new(m_AssetKey);
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        private MessageView m_ViewPrefab;
        private ListPool<MessageView> m_Pool;

        public MessageAnimation(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public void Initialize()
        {
            var asset = m_AssetManager.LoadSync<GameObject>(m_AssetKeyEvaluator);
            if (!asset.HasValue)
            {
                Debug.LogError("[MessageAnimation] In method Initialize, Error in loading MessageView, check prefab");
                return;
            }
            
            m_ViewPrefab = asset.Value.GetComponent<MessageView>();
        
            m_Pool = new ListPool<MessageView>(
                CreateItem, 
                capacity: 3,
                getCallback: (item) => item.SetActive(true),
                releaseCallback: (item) => item.SetActive(false));
        }

        private Optional<MessageView> CreateItem()
        {
            var view = Object.Instantiate(m_ViewPrefab, m_Canvas.GetContent());
            if (view == null)
            {
                Debug.LogError("[MessageAnimation] In method CreateItem, Error in instantiate MessageView, check prefab");
                return Optional<MessageView>.Fail();
            }
            
            return Optional<MessageView>.Success(view);
        }

        public void PlayAnimation(string text)
        {
            var view = m_Pool.Get();
            if (!view.HasValue)
            {
                Debug.LogError($"[MessageAnimation] In method PlayAnimation, class is not init.");
                return;
            }
            
            view.Value.SetText(text.Localize());
            view.Value.RectTransform.anchoredPosition = new Vector2(0, m_StartPositionY);
            view.Value.SetAsLastSibling();
            
            PlayAnimation(view.Value);
        }

        private void PlayAnimation(MessageView view, Action callback = null)
        {
            const float duration = 0.8f;
            
            var transform = view.RectTransform;
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