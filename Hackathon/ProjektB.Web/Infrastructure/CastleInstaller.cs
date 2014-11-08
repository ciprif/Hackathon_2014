using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ProjektB.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Infrastructure
{
    public class CastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            
            container.Register(Component.For<UnitOfWork>().LifestylePerWebRequest());
            container.Register(Component.For<Repository>().LifestyleTransient());
        }
    }
}