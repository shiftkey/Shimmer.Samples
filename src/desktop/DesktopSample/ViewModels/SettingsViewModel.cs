using System.Reactive.Linq;
using System;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class SettingsViewModel : ReactiveValidatedObject, IRoutableViewModel
    {
        public SettingsViewModel(IScreen screen, ISettingsProvider settingsProvider)
        {
            HostScreen = screen;

            SaveCommand = new ReactiveCommand(
                ValidationObservable.Select(next => next.Value),
                initialCondition: false);

            SaveCommand.Subscribe(_ => {
                settingsProvider.UpdateLocation = UpdateLocation;
            });

            UpdateLocation = settingsProvider.UpdateLocation;
        }

        // TODO: custom validation working with RxUI

        public ReactiveCommand SaveCommand { get; set; }

        string _UpdateLocation;
        public string UpdateLocation
        {
            get { return _UpdateLocation; }
            set { this.RaiseAndSetIfChanged(vm => vm.UpdateLocation, value); }
        }

        public string UrlPathSegment { get { return "settings"; } }
        public IScreen HostScreen { get; private set; }
    }
}
