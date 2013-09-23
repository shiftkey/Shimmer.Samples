using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.Client;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ForegroundUpdaterViewModel : ReactiveObject, IRoutableViewModel
    {
        readonly ISettingsProvider settingsProvider;

        readonly Func<ISettingsProvider, UpdateManager> getUpdateManager =
            settingsProvider => {
                var updateManager = new UpdateManager(
                    settingsProvider.UpdateLocation,
                    "ShimmerDesktopDemo",
                    FrameworkVersion.Net40);
                return updateManager;
            };

        ObservableAsPropertyHelper<int> updateCountProperty;
        ObservableAsPropertyHelper<int> progressObservable;

        public ForegroundUpdaterViewModel(
            IScreen screen, 
            ISettingsProvider settingsProvider)
        {
            HostScreen = screen;
            this.settingsProvider = settingsProvider;

            CheckCommand = new ReactiveAsyncCommand();
            CheckCommand.Subscribe(_ => checkForUpdates());

            DownloadCommand = new ReactiveAsyncCommand(
                this.WhenAny(x => x.UpdateInfo, x=> x.Value)
                    .Select(x => x != null && x.ReleasesToApply.Any()));
            DownloadCommand.Subscribe(_ => downloadUpdates());

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));

            updateCountProperty =
                this.WhenAny(x => x.UpdateInfo, x => x.Value)
                .Where(x => x != null)
                .Select(x => x.ReleasesToApply.Count())
                .ToProperty(this, x => x.UpdateCount);

            State = "HomeState";
        }

        void checkForUpdates()
        {
            State = "CheckingState";

            var progress = new Subject<int>();
            progressObservable = progress.ToProperty(
                this,
                vm => vm.Progress);

            if (!settingsProvider.IsUpdateLocationSet) {
                State = "ErrorState";
                ErrorMessage = "You haven't specified where the updates are located. Go back to the settings menu.";
                return;
            }

            var updateManager = getUpdateManager(settingsProvider);

            updateManager.CheckForUpdate(false, progress)
                .Catch<UpdateInfo, ShimmerConfigurationException>(ex => {
                    State = "ErrorState";
                    ErrorMessage = ex.Message;
                    return Observable.Return<UpdateInfo>(null);
                })
                // always be disposing
                .Finally(updateManager.Dispose)
                // this next line isn't necessary in The Real World
                // but i'm adding it here to emphasize
                // the progress bar that you get here
                .Delay(TimeSpan.FromSeconds(1.5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(updateInfo => {

                    UpdateInfo = updateInfo;

                    if (UpdateInfo != null) {
                        State = UpdateInfo.ReleasesToApply.Any()
                                ? "UpdatesFoundState"
                                : "NoChangesState";
                    }
                });
        }


        void downloadUpdates()
        {
            State = "CheckingState";

            var updateManager = getUpdateManager(settingsProvider);

            var progress = new Subject<int>();
            progressObservable = progress.ToProperty(
                this,
                vm => vm.Progress);

            updateManager.DownloadReleases(UpdateInfo.ReleasesToApply, progress)
                         // always be disposing
                         .Finally(updateManager.Dispose)
                         // this next line isn't necessary in The Real World
                         // but i'm adding it here to emphasize
                         // the progress bar that you get here
                         .Delay(TimeSpan.FromSeconds(1.5))
                         .ObserveOn(SynchronizationContext.Current)
                         .Subscribe(_ => {
                             // TODO: 
                             State = "NoChangesState";
                         });
        }

        public int Progress
        {
            get { return progressObservable == null ? 0 : progressObservable.Value; }
        }

        public int UpdateCount { get { return updateCountProperty.Value; }}
        
        UpdateInfo _UpdateInfo;
        public UpdateInfo UpdateInfo
        {
            get { return _UpdateInfo; }
            private set { this.RaiseAndSetIfChanged(ref _UpdateInfo, value); }
        }

        string _State;
        public string State
        {
            get { return _State; }
            private set { this.RaiseAndSetIfChanged(ref _State, value); }
        }

        string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _ErrorMessage, value); }
        }

        public string UrlPathSegment { get { return "foreground"; }}
        public IScreen HostScreen { get; private set; }

        public ReactiveAsyncCommand CheckCommand { get; private set; }
        public ReactiveAsyncCommand DownloadCommand { get; private set; }
        public ReactiveAsyncCommand BackCommand { get; private set; }
    }
}