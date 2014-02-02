using ReactiveUI;
using ReactiveUI.Routing;

namespace Squirrel.DesktopDemo.ViewModels
{
    public class BackgroundUpdaterViewModel : ReactiveObject, IRoutableViewModel
    {
        public BackgroundUpdaterViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment { get { return "background"; } }
        public IScreen HostScreen { get; private set; }
    }
}