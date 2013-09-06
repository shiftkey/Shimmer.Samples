using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Logic
{
    public interface IUpdaterStrategy
    {
        IUpdaterViewModel UseBackground();
        IUpdaterViewModel UseForeground();
    }

    public class BackgroundUpdater : IUpdaterStrategy
    {
        public IUpdaterViewModel UseBackground()
        {
            return new BackgroundUpdaterViewModel();
        }

        public IUpdaterViewModel UseForeground()
        {
            return new ForegroundUpdaterViewModel();
        }
    }
}
