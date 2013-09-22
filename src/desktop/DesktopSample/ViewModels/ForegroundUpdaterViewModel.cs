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

        ObservableAsPropertyHelper<int> progressObservable;

        public ForegroundUpdaterViewModel(
            IScreen screen, 
            ISettingsProvider settingsProvider)
        {
            HostScreen = screen;
            this.settingsProvider = settingsProvider;

            NextCommand = new ReactiveAsyncCommand();
            NextCommand.Subscribe(_ => CheckForUpdates());

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));

            State = "HomeState";
        }

        private void CheckForUpdates()
        {
            State = "CheckingState";

            var progress = new Subject<int>();
            progressObservable = progress.ToProperty(
                this,
                vm => vm.Progress);


            if (!settingsProvider.IsUpdateLocationSet) {
                State = "Error";
                ErrorMessage = "You haven't specified where the updates are located. Go back to the settings menu.";
                // TODO: navigate user to configuration?
                return;
            }

            var updateManager = new UpdateManager(
                settingsProvider.UpdateLocation,
                "ShimmerDesktopDemo",
                FrameworkVersion.Net40);

            updateManager.CheckForUpdate(false, progress)
                .Catch<UpdateInfo, ShimmerConfigurationException>(ex => {
                    State = "Error";
                    ErrorMessage = ex.Message;
                    return Observable.Return<UpdateInfo>(null);
                })
                .Finally(updateManager.Dispose)
                // this next line isn't necessary in The Real World
                // but i'm adding it here to emphasize
                // the progress bar that you get here
                .Delay(TimeSpan.FromSeconds(3)) 
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(updateInfo => {

                    UpdateInfo = updateInfo;

                    if (updateInfo != null) {
                        if (!UpdateInfo.ReleasesToApply.Any()) {
                            State = "NoChangesState";
                            return;
                        } else {
                            State = "ChangesToApply";
                            return;
                        }
                    }
                });
        }

        public int Progress
        {
            get { return progressObservable == null ? 0 : progressObservable.Value; }
        }
        
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

        public ReactiveAsyncCommand NextCommand { get; private set; }
        public ReactiveAsyncCommand BackCommand { get; private set; }

    }
}