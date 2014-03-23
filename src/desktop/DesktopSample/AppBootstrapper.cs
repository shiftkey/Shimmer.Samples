﻿using System.Collections.Generic;
using System.Reactive.Linq;
using Autofac;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Squirrel.Client;
using Squirrel.DesktopDemo.Logic;
using Squirrel.DesktopDemo.ViewModels;

namespace Squirrel.DesktopDemo
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper(IRoutingState testRouter = null, ContainerBuilder builder = null)
        {
            Router = testRouter ?? new RoutingState();

            LogHost.Default.Level = LogLevel.Debug;

            ReactiveUIMicro.RxApp.ConfigureFileLogging("SquirrelDesktopDemo");

            var containerBuilder = builder ?? CreateStandardContainer();

            // AppBootstrapper is a global variable, so bind up
            containerBuilder.RegisterInstance(this)
                            .As<IScreen>()
                            .SingleInstance();
            
            // we want to create our UpdateManager in the same way each time
            containerBuilder.Register(ctx => {
                var settings = ctx.Resolve<ISettingsProvider>();
                return new UpdateManager(settings.UpdateLocation, "SquirrelDesktopDemo", FrameworkVersion.Net40);
            }).As<UpdateManager>();

            // by default i want this app to use the test feed
            // but you can change this value in the settings
            var settingsProvider = new SettingsProvider();
            settingsProvider.UpdateLocation = "http://shimmer-demo.herokuapp.com/demo/";

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

            UserError.RegisterHandler(ex => {
                var errorVm = container.Resolve<ErrorViewModel>();
                errorVm.Message = string.Format("{0}\r\n\r\n{1}", ex.ErrorMessage, ex.InnerException);

                Router.Navigate.Execute(errorVm);
                return Observable.Return(RecoveryOptionResult.CancelOperation);
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
