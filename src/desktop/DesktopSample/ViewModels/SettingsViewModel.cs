using System.Reactive.Linq;
using System;
using ReactiveUI;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.Logic;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class SettingsViewModel : ReactiveValidatedObject
    {
        public SettingsViewModel(ISettingsProvider settingsProvider)
        {
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
    }
}
