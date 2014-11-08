using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Resolvers;

namespace ProjektB.Web.App_Start
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        readonly IWindsorContainer container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            this.container = container;
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer();

            //this is a hack!
            MvcApplication.Container = container;

            //setup support for Lazy
            container.Register(
                Castle.MicroKernel.Registration.Component.For
               <ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>());

            container.Install(FromAssembly.This());
            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}