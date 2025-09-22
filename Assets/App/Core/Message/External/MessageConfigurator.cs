using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.CubeDragger.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class MessageConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<MessageController>().AsSingle();
            
            FsmRegistrar.Register<MessageController>(FSMStage.CoreInitStage, 5000);
        }
    }
}
