using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace MUTDOD.Server.DataServer.DataServerBase
{
    public class DataServerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Install(Configuration.FromXmlFile("DataServerConfiguration.xml"));
        }
    }
}
