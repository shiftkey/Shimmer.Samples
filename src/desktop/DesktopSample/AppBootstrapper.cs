using System.Collections.Generic;
using Autofac;
using ReactiveUI;
using ReactiveUI.Routing;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper(IRoutingState testRouter = null, ContainerBuilder builder = null)
        {
            Router = testRouter ?? new RoutingState();

            LogHost.Default.Level = LogLevel.Debug;

            var containerBuilder = builder ?? CreateStandardContainer();

            // AppBootstrapper is a global variable, so bind up 

            containerBuilder.RegisterInstance(this)
                            .As<IScreen>()
                            .SingleInstance();

            var container = containerBuilder.Build();

            RxApp.ConfigureServiceLocator(
                (iface, contract) => container.Resolve(iface),
                (iface, contract) => {
                    var collection = typeof(IEnumerable<>).MakeGenericType(iface);
                    return container.Resolve(collection) as IEnumerable<object>;
                },
                (realClass, iface, contract) => {
                    var newBuilder = new ContainerBuilder();
                    newBuilder.RegisterType(realClass).As(iface);
                    newBuilder.Update(container);
                });

            var shell = container.Resolve<ShellViewModel>();
            Router.Navigate.Execute(shell);
        }

        static ContainerBuilder CreateStandardContainer()
        {
            var container = new ContainerBuilder();
            container.RegisterAssemblyTypes(typeof(AppBootstrapper).Assembly)
                     .AsSelf()
                     .AsImplementedInterfaces();
            return container;
        }

        public IRoutingState Router { get; private set; }
    }
}
