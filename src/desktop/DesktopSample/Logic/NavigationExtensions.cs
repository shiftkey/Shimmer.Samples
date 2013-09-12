using ReactiveUI.Routing;

namespace Shimmer.DesktopDemo.Logic
{
    public static class NavigationExtensions
    {
        public static void Navigate(this IScreen screen, object viewModel)
        {
            if (screen != null
                && screen.Router != null
                && screen.Router.Navigate != null) {
                screen.Router.Navigate.Execute(viewModel);
            }
        }
    }
}
