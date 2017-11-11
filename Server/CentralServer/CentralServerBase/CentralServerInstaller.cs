using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MUTDOD.Common;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.Tools.Logger;

namespace MUTDOD.Server.CentralServer.CentralServerBase
{
    public class CentralServerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(Configuration.FromXmlFile("CentralServerConfiguration.xml"));
        }
    }
}
