using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.Core.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class CoreUIConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<CoreUIController>().AsSingle();
            
            FsmRegistrar.Register<CoreUIController>(FSMStage.CoreInitStage, 10);
        }
    }
}