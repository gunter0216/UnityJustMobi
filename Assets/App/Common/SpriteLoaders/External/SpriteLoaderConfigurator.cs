using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.SpriteLoaders.External
{
    [Configurator(ContextConstants.GlobalContext)]    
    public class SpriteLoaderConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<SpriteLoader>().AsSingle();
            
            FsmRegistrar.Register<SpriteLoader>(FSMStage.StartInitStage, 0);
        }
    }
}