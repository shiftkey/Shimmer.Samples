using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.Client;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ForegroundUpdaterViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        readonly UpdateManager updateManager;

        public ForegroundUpdaterViewModel(
            IScreen screen, 
            ISettingsProvider settingsProvider)
        {
            HostScreen = screen;

            NextCommand = new ReactiveAsyncCommand();
            NextCommand.Subscribe(_ =>
            {
                SetVisualState("Checking");

                var progress = new Subject<int>();
                progressObservable = progress.ToProperty(
                    this,
                    vm => vm.Progress,
                    setViaReflection: false);

                UpdateInfo = updateManager.CheckForUpdate(false, progress).Wait();

                SetVisualState("Checked");

            });

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));

            updateManager = new UpdateManager(
                settingsProvider.UpdateLocation,
                "ShimmerDesktopDemo",
                FrameworkVersion.Net40);
        }

        UpdateInfo _UpdateInfo;
        public UpdateInfo UpdateInfo
        {
            get { return _UpdateInfo; }
            private set { this.RaiseAndSetIfChanged(ref _UpdateInfo, value); }
        }

        ObservableAsPropertyHelper<int> progressObservable;

        public int Progress { get { return progressObservable.Value; } }

        void SetVisualState(string state)
        {
        }

        public string UrlPathSegment { get { return "foreground"; }}
        public IScreen HostScreen { get; private set; }

        public ReactiveAsyncCommand NextCommand { get; private set; }
        public ReactiveAsyncCommand BackCommand { get; private set; }
        
        public void Dispose()
        {
            if (updateManager != null)
                updateManager.Dispose();
        }
    }
}