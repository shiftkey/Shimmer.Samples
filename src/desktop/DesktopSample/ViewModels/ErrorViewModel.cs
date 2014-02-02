using System;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace Squirrel.DesktopDemo.ViewModels
{
    public class ErrorViewModel : ReactiveObject, IRoutableViewModel
    {
        public ErrorViewModel(IScreen screen,
            Func<ShellViewModel> getShellViewModel)
        {
            HostScreen = screen;

            Back = new ReactiveAsyncCommand();
            Back.Subscribe(_ => HostScreen.Router.NavigateAndReset.Execute(getShellViewModel()));
        }

        public string UrlPathSegment { get { return "error"; } }
        public IScreen HostScreen { get; private set; }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { this.RaiseAndSetIfChanged(ref _Message, value); }
        }

        public ReactiveAsyncCommand Back { get; private set; }
    }
}
