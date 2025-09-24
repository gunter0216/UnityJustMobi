﻿using System;
using System.Collections.Generic;
using App.Common.AssemblyManager.Runtime;
using App.Common.Data.Runtime;
using App.Common.FSM.External;
using App.Core.Startups.External.Attributes;
using Castle.Core.Internal;
using UnityEngine;
using Zenject;

namespace App.Core.Startups.External
{
    /// <summary>
    /// Отвечает за хранение и запуск конфигураторов, которые закидывают своим классы в DI контейнер, регистрируют их в FSM и DataRegistrar
    /// </summary>
    public class ConfiguratorsManager
    {
        private readonly FSMRegistrar m_FsmRegistrar;
        private readonly DataRegistrar m_DataRegistrar;

        private readonly Dictionary<int, List<Configurator>> m_Configurators = new();

        public ConfiguratorsManager(
            IReadOnlyList<AttributeNode> configurators, 
            FSMRegistrar fsmRegistrar,
            DataRegistrar dataRegistrar)
        {
            m_FsmRegistrar = fsmRegistrar;
            m_DataRegistrar = dataRegistrar;

            SetConfigurators(configurators);
        }

        private void SetConfigurators(IReadOnlyList<AttributeNode> configurators)
        {
            foreach (var configurator in configurators)
            {
                SetConfigurator(configurator);
            }
        }

        private void SetConfigurator(AttributeNode node)
        {
            var attributes = node.Holder.GetAttributes<ConfiguratorAttribute>();
            var instance = Activator.CreateInstance(node.Holder);
            foreach (var attribute in attributes)
            {
                if (instance is Configurator configurator)
                {
                    SetConfigurator(attribute.Context, configurator);
                }
                else
                {
                    Debug.LogError($"Configurator {node.Holder} is not Configurator");
                    return;
                }
            }
        }

        private void SetConfigurator(int context, Configurator configurator)
        {
            if (!m_Configurators.TryGetValue(context, out var configurators))
            {
                configurators = new List<Configurator>(1);
                m_Configurators.Add(context, configurators);
            }
            
            configurators.Add(configurator);
        }

        public void RunConfigurator(int context, DiContainer container)
        {
            if (!m_Configurators.TryGetValue(context, out var configurators))
            {
                return;
            }

            foreach (var configurator in configurators)
            {
                configurator.SetDiContainer(container);
                configurator.SetFSMRegistrator(m_FsmRegistrar);
                configurator.SetDataRegistrator(m_DataRegistrar);
                configurator.Configuration();
            }
        }
    }
}