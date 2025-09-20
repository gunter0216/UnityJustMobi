using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.StartScene.External
{
    [Configurator(ContextConstants.StartContext)]    
    public class StartSceneConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<StartSceneController>().AsSingle();
            
            FsmRegistrar.Register<StartSceneController>(FSMStage.StartInitStage, 100_100);
        }
    }
}