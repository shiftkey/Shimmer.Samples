using System;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        public ShellViewModel(IScreen screen, Func<SettingsViewModel> getSettings)
        {
            HostScreen = screen;

            SettingsCommand = new ReactiveCommand();
            SettingsCommand.Subscribe(next =>
            {
                var viewModel = getSettings();
                HostScreen.Router.Navigate.Execute(viewModel);
            });
        }

        public string UrlPathSegment { get { return "shell"; } }
        public IScreen HostScreen { get; private set; }
        public ReactiveCommand SettingsCommand { get; private set; }
    }
}
