using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        public ShellViewModel(
            IScreen screen,
            Func<SettingsViewModel> getSettings,
            Func<CheckForUpdatesViewModel> getForegroundUpdater,
            Func<BackgroundUpdaterViewModel> getBackgroundUpdater)
        {
            HostScreen = screen;

            SettingsCommand = new ReactiveAsyncCommand();
            SettingsCommand.RegisterAsyncAction(o => {
                var viewModel = getSettings();
                HostScreen.Navigate(viewModel);
            });

            UpdateBasicsCommand = new ReactiveAsyncCommand();
            UpdateBasicsCommand.RegisterAsyncAction(o => {
                var viewModel = getForegroundUpdater();
                HostScreen.Navigate(viewModel);
            });

            BackgroundUpdaterCommand = new ReactiveAsyncCommand(Observable.Return(false));
            BackgroundUpdaterCommand.RegisterAsyncAction(o => {
                var viewModel = getBackgroundUpdater();
                HostScreen.Navigate(viewModel);
            });
        }

        public string UrlPathSegment { get { return "shell"; } }
        public IScreen HostScreen { get; private set; }

        public ReactiveAsyncCommand SettingsCommand { get; private set; }
        public ReactiveAsyncCommand BackgroundUpdaterCommand { get; private set; }
        public ReactiveAsyncCommand UpdateBasicsCommand { get; private set; }
    }

}
