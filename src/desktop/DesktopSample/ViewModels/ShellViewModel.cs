using ReactiveUI;
using ReactiveUI.Routing;

namespace Shimmer.DesktopDemo.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        public ShellViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment { get { return "shell"; } }
        public IScreen HostScreen { get; private set; }
    }
}
