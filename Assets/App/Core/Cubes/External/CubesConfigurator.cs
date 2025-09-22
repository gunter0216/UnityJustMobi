using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.Core.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class CubesConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<CubesController>().AsSingle();
            
            FsmRegistrar.Register<CubesController>(FSMStage.CoreInitStage, 10);
        }
    }
}
