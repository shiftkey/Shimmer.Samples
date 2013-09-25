using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.Client;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class DownloadUpdatesViewModel : ReactiveObject, IRoutableViewModel
    {
        readonly Lazy<ApplyUpdatesViewModel> getApplyViewModel;

        readonly ObservableAsPropertyHelper<int> progressObs;
        readonly ObservableAsPropertyHelper<int> updateCountObs;
        readonly ObservableAsPropertyHelper<string> latestVersionObs;
        readonly ObservableAsPropertyHelper<string> updateSizeObs;

        readonly Subject<int> progress;

        readonly Func<UpdateManager> getUpdateManager;
  
        public DownloadUpdatesViewModel(
            IScreen screen,
            Func<UpdateManager> getUpdateManager,
            Lazy<ApplyUpdatesViewModel> getApplyViewModel)
        {
            HostScreen = screen;
            this.getUpdateManager = getUpdateManager;
            this.getApplyViewModel = getApplyViewModel;

            Download = new ReactiveAsyncCommand();
            Download.Subscribe(_ => downloadUpdates());

            progress = new Subject<int>();
            progressObs = progress.ToProperty(
                this,
                vm => vm.Progress);

            var updateInfoChanges = 
                this.WhenAny(vm => vm.UpdateInfo, x => x.Value)
                    .Where(info => info != null);

            updateCountObs =
                updateInfoChanges
                    .Select(info => info.ReleasesToApply.Count())
                    .ToProperty(this, vm => vm.UpdateCount);

            updateSizeObs =
                updateInfoChanges
                    .Select(info => info.ReleasesToApply.Sum(x => x.Filesize))
                    .Select(total => String.Format("({0:n0} bytes)", total))
                    .ToProperty(this, vm => vm.UpdateSize);

            latestVersionObs = updateInfoChanges
                .Select(info => info.FutureReleaseEntry.Version.ToString())
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .ToProperty(this, vm => vm.LatestVersion);
        }

        void downloadUpdates()
        {
            State = "ProgressState";

            var updateManager = getUpdateManager();
            updateManager.DownloadReleases(UpdateInfo.ReleasesToApply, progress)
                // always be disposing
                .Finally(updateManager.Dispose)
                .Catch<Unit, Exception>(ex => {
                    UserError.Throw(new UserError("Something unexpected happened", innerException: ex));
                    return Observable.Return(Unit.Default);
                })
                // this next line isn't necessary in The Real World
                // but i'm adding it here to emphasize
                // the progress bar that you get here
                .Delay(TimeSpan.FromSeconds(1.5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(_ =>
                {
                    var viewModel = getApplyViewModel.Value;
                    viewModel.UpdateInfo = UpdateInfo;
                    HostScreen.Navigate(viewModel);
                });
        }

        public string UrlPathSegment { get { return "check-updates"; } }
        public IScreen HostScreen { get; private set; }

        public int Progress { get { return progressObs.Value; } }
        public int UpdateCount { get { return updateCountObs.Value; } }

        public string UpdateSize 
        {
            get { return updateSizeObs.Value; }
            set { /* pay no attention to what this hack is here for */ }
        }

        public string LatestVersion 
        {
            get { return latestVersionObs.Value; }
            set { /* pay no attention to what this hack is here for */ }
        }

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

        public ReactiveAsyncCommand Download { get; private set; }
    }
}
