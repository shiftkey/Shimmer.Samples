using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Squirrel.DesktopDemo.ViewModels;

namespace Squirrel.DesktopDemo.Views
{
    public partial class DownloadUpdatesView : IViewFor<DownloadUpdatesViewModel>
    {
        public DownloadUpdatesView()
        {
            InitializeComponent();

            this.WhenAny(x => x.ViewModel.State, x => x.Value)
                .Where(x => x != null)
                .Subscribe(x => VisualStateManager.GoToElementState(Grid, x, true));

            this.BindCommand(ViewModel,
                vm => vm.Download,
                view => view.DownloadUpdates);

            this.Bind(ViewModel,
                vm => vm.Progress,
                view => view.CheckingProgress.Value);

            this.Bind(ViewModel,
                vm => vm.UpdateCount,
                view => view.UpdatesCount.Text);

            this.Bind(ViewModel,
                vm => vm.LatestVersion,
                view => view.LatestVersion.Text);

            this.Bind(ViewModel,
                vm => vm.UpdateSize,
                view => view.UpdatesSize.Text);
        }

        public DownloadUpdatesViewModel ViewModel
        {
            get { return (DownloadUpdatesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DownloadUpdatesViewModel), typeof(DownloadUpdatesView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DownloadUpdatesViewModel)value; }
        }
    }
}
