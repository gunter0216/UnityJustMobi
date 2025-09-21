using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.Core.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class CoreConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<CoreController>().AsSingle();
            
            FsmRegistrar.Register<CoreController>(FSMStage.CoreInitStage, 10000);
        }
    }
}