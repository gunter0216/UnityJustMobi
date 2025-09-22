using App.Common.FSM.External;
using App.Core.Core.External.Presenter;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.Core.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class TowerConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<TowerController>().AsSingle();
            
            FsmRegistrar.Register<TowerController>(FSMStage.CoreInitStage, 100);
        }
    }
}
