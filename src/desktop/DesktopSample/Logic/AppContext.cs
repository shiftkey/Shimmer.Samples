using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using ReactiveUI;

namespace Squirrel.DesktopDemo.Logic
{
    public interface IAppContext
    {
        IScheduler DispatcherScheduler { get; }
        IFileSystem FileSystem { get; }
    }

    public class AppContext : IAppContext
    {
        // TODO: should these be fields and instantiated once only?

        public IScheduler DispatcherScheduler
        {
            get { return new DispatcherScheduler(App.Current.Dispatcher);}
        }

        public IFileSystem FileSystem { get {return new FileSystem(); } }
    }
}
