﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Common.AssemblyManager.Runtime
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private readonly Dictionary<Type, List<AttributeNode>> m_AttributeToTypes;

        public AssemblyProvider(Dictionary<Type, List<AttributeNode>> attributeToTypes)
        {
            m_AttributeToTypes = attributeToTypes;
        }

        public IReadOnlyList<AttributeNode> GetTypes<T>() where T : Attribute
        {
            if (m_AttributeToTypes.TryGetValue(typeof(T), out var nodes))
            {
                return nodes;
            }

            Debug.LogError($"{nameof(T)} types not found.");
            return new List<AttributeNode>();
        }
    }
}