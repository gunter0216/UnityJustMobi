using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.ApplicationQuit.External
{
    [Configurator(ContextConstants.GlobalContext)]
    public class ApplicationQuitConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<ApplicationQuitController>().AsSingle();
            
            FsmRegistrar.Register<ApplicationQuitController>(FSMStage.StartInitStage, 0);
        }
    }
}