using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.Client;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ApplyUpdatesViewModel : ReactiveObject, IRoutableViewModel
    {
        readonly Func<UpdateManager> getUpdateManager;

        readonly ObservableAsPropertyHelper<int> progressObservable;
        readonly Subject<int> progress;

        public ApplyUpdatesViewModel(
            IScreen screen,
            Func<UpdateManager> getUpdateManager)
        {
            this.getUpdateManager = getUpdateManager;
            HostScreen = screen;

            Apply = new ReactiveAsyncCommand();
            Apply.Subscribe(_ => applyUpdates());

            Restart = new ReactiveAsyncCommand();
            Restart.Subscribe(_ => restart());

            progress = new Subject<int>();
            progressObservable = progress.ToProperty(this, vm => vm.Progress);
        }

        void applyUpdates()
        {
            State = "ProgressState";

            var updateManager = getUpdateManager();
            updateManager.ApplyReleases(UpdateInfo, progress)
                .Finally(updateManager.Dispose)
                .Catch<List<string>, Exception>(ex =>
                {
                    UserError.Throw(new UserError("Something unexpected happened", innerException: ex));
                    return Observable.Return(new List<string>());
                })
                // this next line isn't necessary in The Real World
                // but i'm adding it here to emphasize
                // the progress bar that you get here
                .Delay(TimeSpan.FromSeconds(1.5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(results => {
                    State = "RestartState";
                });
        }

        void restart()
        {
            // TODO: make a trick to restart the app
        }

        public string UrlPathSegment { get { return "apply-updates"; } }
        public IScreen HostScreen { get; private set; }

        UpdateInfo _UpdateInfo;
        public UpdateInfo UpdateInfo
        {
            get { return _UpdateInfo; }
            set { this.RaiseAndSetIfChanged(ref _UpdateInfo, value); }
        }

        string _State;
        public string State
        {
            get { return _State; }
            private set { this.RaiseAndSetIfChanged(ref _State, value); }
        }

        public int Progress { get { return progressObservable.Value; } }

        public ReactiveAsyncCommand Apply { get; private set; }
        public ReactiveAsyncCommand Restart { get; private set; }
    }
}
