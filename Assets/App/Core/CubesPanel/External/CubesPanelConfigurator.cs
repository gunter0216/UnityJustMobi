using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Core.CubesPanel.External
{
    [Configurator(ContextConstants.CoreContext)]    
    public class CubesPanelConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<CubesPanelController>().AsSingle();
            
            FsmRegistrar.Register<CubesPanelController>(FSMStage.CoreInitStage, 10000);
        }
    }
}