using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.Hole.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class HoleConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<HoleController>().AsSingle();
            
            FsmRegistrar.Register<HoleController>(FSMStage.CoreInitStage, 100);
        }
    }
}
