using App.Common.Utilities.Utility.Runtime;
using App.Core.CoreUI.External.View;
using UnityEngine;

namespace App.Core.CubesPanel.External.Presenter.Fabric
{
    public class CubeViewCreator
    {
        private readonly CoreCubesView m_View;

        public CubeViewCreator(CoreCubesView view)
        {
            m_View = view;
        }

        public Optional<CubeView> Create(Transform parent)
        {
            var view = Object.Instantiate(m_View.CubeViewPrefab, parent);
            if (view == null)
            {
                return Optional<CubeView>.Fail();
            }
            
            return Optional<CubeView>.Success(view);
        }
    }
}