using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime.Extensions;
using DG.Tweening;
using UnityEngine;

namespace App.Core.Main.External.Animations
{
    public class RotateAnimation : IDisposable
    {
        public const float LoadingRotateTime = RotateAnimationCreator.LoadingRotateTime;
        public const float DefaultRotateTime = RotateAnimationCreator.DefaultRotateTime;
        
        private readonly List<Transform> m_Targets;
        private readonly float m_RotateTime = DefaultRotateTime;
        private Sequence m_Sequence;
        private readonly bool m_Flip = false;
        private readonly RotateAnimationCreator m_RotateAnimationCreator = new();

        public RotateAnimation()
        {
        }
        
        public RotateAnimation(
            Transform target, 
            float rotateTime = RotateAnimationCreator.DefaultRotateTime,
            bool flip = false)
        {
            m_Targets = new List<Transform>()
            {
                target
            };

            m_Flip = flip;
            m_RotateTime = rotateTime;
        }
        
        public RotateAnimation(List<Transform> targets, float rotateTime = RotateAnimationCreator.DefaultRotateTime)
        {
            m_Targets = targets;
            
            m_RotateTime = rotateTime;
        }

        public bool IsActive()
        {
            return m_Sequence != null && m_Sequence.IsActive();
        }
        
        public void StartAnimation()
        {
            StartAnimation(m_Targets);
        }

        public void StartAnimation(Transform target)
        {
            StartSingleAnimation(target);
        }
        
        public void StartAnimation(List<Transform> targets)
        {
            if (targets.IsNullOrEmpty())
            {
                return;
            }
            
            if (targets.Count == 1)
            {
                StartSingleAnimation(targets[0]);
            }
            else
            {
                StartMultiAnimation(targets);
            }
        }

        private void StartSingleAnimation(Transform target)
        {
            StopAnimation();
            m_Sequence = m_RotateAnimationCreator.Create(target, m_RotateTime, flip: m_Flip);
        }

        private void StartMultiAnimation(List<Transform> targets)
        {
            StopAnimation();
            m_Sequence = DOTween.Sequence();
            foreach (var target in targets)
            {
                m_Sequence.Join(m_RotateAnimationCreator.Create(target, m_RotateTime, flip: m_Flip));
            }

            m_Sequence.SetLoops(-1, LoopType.Restart);
        }

        public void StopAnimation()
        {
            m_Sequence?.Kill();
        }

        public void Dispose()
        {
            StopAnimation();
        }
    }
}