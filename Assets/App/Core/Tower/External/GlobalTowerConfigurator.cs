using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Core.Tower.External.Data;

namespace App.Core.Tower.External
{
    [Configurator(ContextConstants.GlobalContext)]    
    public class GlobalTowerConfigurator : Configurator
    {
        public override void Configuration()
        {
            DataRegistrar.Register<TowerData>();
        }
    }
}