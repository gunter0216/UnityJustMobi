using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Audio.External
{
    public class SoundManager : IInitSystem, IDisposable, ISoundManager
    {
        private readonly IAssetManager m_AssetManager;
        
        private List<AudioSource> m_Sources;
        private HashSet<string> m_LoadedClips;

        public SoundManager(IAssetManager assetManager)
        {
            m_AssetManager = assetManager;
        }

        public void Init()
        {
            var capacity = 8;
            m_Sources = new List<AudioSource>(capacity);
            m_LoadedClips = new HashSet<string>();
            var gameObject = new GameObject("SoundProvider");
            for (int i = 0; i < capacity; ++i)
            {
                var src = gameObject.AddComponent<AudioSource>();
                src.playOnAwake = false;
                m_Sources.Add(src);
            }
        }

        public void Play(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            
            var audioClip = m_AssetManager.LoadSync<AudioClip>(new StringKeyEvaluator(key));
            if (!audioClip.HasValue)
            {
                Debug.LogError($"SoundManager: AudioClip with key '{key}' not found.");
                return;
            }

            m_LoadedClips.Add(key);

            var audioSource = GetAvailableSource();
            if (!audioSource.HasValue)
            {
                return;
            }

            audioSource.Value.PlayOneShot(audioClip.Value);
        }

        private Optional<AudioSource> GetAvailableSource()
        {
            foreach (var audioSource in m_Sources)
            {
                if (!audioSource.isPlaying)
                {
                    return Optional<AudioSource>.Success(audioSource);
                }
            }

            return Optional<AudioSource>.Fail();
        }

        public void Dispose()
        {
            foreach (var clip in m_LoadedClips)
            {
                m_AssetManager.UnloadAsset(new StringKeyEvaluator(clip));
            }
        }
    }
}