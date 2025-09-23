using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Events.External
{
    [Configurator(ContextConstants.GlobalContext)]
    public class EventManagerConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<EventManager>().AsSingle();
        }
    }
}