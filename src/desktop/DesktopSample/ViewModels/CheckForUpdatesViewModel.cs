using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Squirrel.Client;
using Squirrel.DesktopDemo.Logic;

namespace Squirrel.DesktopDemo.ViewModels
{
    public class CheckForUpdatesViewModel : ReactiveObject, IRoutableViewModel
    {
        readonly ISettingsProvider settingsProvider;
        readonly Lazy<DownloadUpdatesViewModel> getDownloadViewModel;

        readonly Func<UpdateManager> getUpdateManager;

        readonly ObservableAsPropertyHelper<int> progressObservable;
        readonly Subject<int> progress;

        public CheckForUpdatesViewModel(
            IScreen screen,
            ISettingsProvider settingsProvider,
            Func<UpdateManager> getUpdateManager,
            Lazy<DownloadUpdatesViewModel> getDownloadViewModel)
        {
            HostScreen = screen;
            this.settingsProvider = settingsProvider;
            this.getUpdateManager = getUpdateManager;
            this.getDownloadViewModel = getDownloadViewModel;

            CheckCommand = new ReactiveAsyncCommand();
            CheckCommand.Subscribe(_ => checkForUpdates());

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));

            progress = new Subject<int>();
            progressObservable = progress.ToProperty(
                this,
                vm => vm.Progress);
        }

        void checkForUpdates()
        {
            State = "ProgressState";

            if (!settingsProvider.IsUpdateLocationSet) {
                UserError.Throw("You haven't specified where the updates are located. Go back to the settings menu.");
                return;
            }

            var updateManager = getUpdateManager();
            updateManager.CheckForUpdate(!UseDeltaPackages, progress)
                .Catch<UpdateInfo, SquirrelConfigurationException>(ex => {
                    UserError.Throw(new UserError("Something unexpected happened", innerException: ex));
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
                    if (updateInfo == null) return;
                    if (updateInfo.ReleasesToApply.Any()) {
                        var viewModel = getDownloadViewModel.Value;
                        viewModel.UpdateInfo = updateInfo;
                        HostScreen.Navigate(viewModel);
                    } else {
                        State = "NoChangesState";
                    }
                });
        }

        public int Progress
        {
            get { return progressObservable.Value; }
        }

        string _State;
        public string State
        {
            get { return _State; }
            private set { this.RaiseAndSetIfChanged(ref _State, value); }
        }

        bool _UseDeltaPackages = true;
        public bool UseDeltaPackages
        {
            get { return _UseDeltaPackages; }
            set { this.RaiseAndSetIfChanged(ref _UseDeltaPackages, value); }
        }

        public ReactiveAsyncCommand BackCommand { get; set; }
        public ReactiveAsyncCommand CheckCommand { get; private set; }

        public string UrlPathSegment { get { return "check-for-updates"; } }
        public IScreen HostScreen { get; private set; }
    }
}
