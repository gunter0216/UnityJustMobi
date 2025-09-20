using App.Common.Data.Runtime;
using App.Common.FSM.External;
using Zenject;

namespace App.Core.Startups.External
{
    public abstract class Configurator
    {
        private DiContainer m_Container;
        private FSMRegistrar m_FsmRegistrar;
        private DataRegistrar m_DataRegistrar;

        protected DiContainer Container => m_Container;
        protected FSMRegistrar FsmRegistrar => m_FsmRegistrar;
        protected DataRegistrar DataRegistrar => m_DataRegistrar;

        public void SetDiContainer(DiContainer container)
        {
            m_Container = container;
        }
        
        public void SetFSMRegistrator(FSMRegistrar fsmRegistrar)
        {
            m_FsmRegistrar = fsmRegistrar;
        }

        public void SetDataRegistrator(DataRegistrar dataRegistrar)
        {
            m_DataRegistrar = dataRegistrar;
        }

        public abstract void Configuration();
    }
}