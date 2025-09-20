using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Audio.External
{
    [Configurator(ContextConstants.CoreContext)]
    public class SoundConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<SoundManager>().AsSingle();
            
            FsmRegistrar.Register<SoundManager>(FSMStage.CoreInitStage, 0);
        }
    }
}