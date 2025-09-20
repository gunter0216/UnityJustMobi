using App.Common.AssemblyManager.External;
using App.Common.Data.Runtime;
using App.Common.FSM.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using Zenject;

namespace App.Core.Startups.External
{
    public class GlobalStartup : MonoInstaller<StartSceneStartup>
    {
        public override void InstallBindings()
        {
            var assemblyProvider = new AssemblyManager()
                .CreateAssemblyProviderBuilder()
                .AddAttribute<ConfiguratorAttribute>()
                .Build();
            
            var configurators = assemblyProvider.GetTypes<ConfiguratorAttribute>();
            var fsmRegistrar = new FSMRegistrar();
            var dataRegistrar = new DataRegistrar();
            
            var configuratorsManager = new ConfiguratorsManager(configurators, fsmRegistrar, dataRegistrar);
            configuratorsManager.RunConfigurator(ContextConstants.GlobalContext, Container);
            
            Container.BindInstance(configuratorsManager);
            Container.BindInstance(fsmRegistrar);
            Container.BindInstance(dataRegistrar);
        }
    }
}