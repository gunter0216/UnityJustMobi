﻿using System;
using System.Collections.Generic;
using App.Common.FSM.Runtime;
using UnityEngine;

namespace App.Common.FSM.External
{
    public class FSMRegistrar
    {
        private Dictionary<Type, List<FSMIItemnfo>> m_Info = new();
        
        public void Register<T>(FSMStage stage, int order) where T : class
        {
            var type = typeof(T);
            if (!m_Info.TryGetValue(type, out var stageInfo))
            {
                stageInfo = new List<FSMIItemnfo>(1);
                m_Info.Add(type, stageInfo);
            }

            stageInfo.Add(new FSMIItemnfo((int)stage, order));
        }

        public Dictionary<Type, List<FSMIItemnfo>> GetInfo()
        {
            return m_Info;
        }
    }
}