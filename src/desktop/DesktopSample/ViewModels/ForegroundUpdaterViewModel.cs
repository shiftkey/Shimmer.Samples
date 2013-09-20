using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ForegroundUpdaterViewModel : ReactiveObject, IRoutableViewModel
    {
        public ForegroundUpdaterViewModel(IScreen screen)
        {
            HostScreen = screen;

            NextCommand = new ReactiveAsyncCommand();
            NextCommand.Subscribe(_ =>
            {
                // TODO: something important eh?
            });

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));
        }

        public string UrlPathSegment { get { return "foreground"; }}
        public IScreen HostScreen { get; private set; }

        public ReactiveAsyncCommand NextCommand { get; private set; }
        public ReactiveAsyncCommand BackCommand { get; private set; }
    }
}