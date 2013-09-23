using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class CheckForUpdatesView : IViewFor<CheckForUpdatesViewModel>
    {
        public CheckForUpdatesView()
        {
            InitializeComponent();

            this.WhenAny(x => x.ViewModel.State, x => x.Value)
                .Where(x => x != null)
                .Subscribe(x => VisualStateManager.GoToElementState(Grid, x, true));

            this.BindCommand(ViewModel,
                vm => vm.CheckCommand,
                view => view.Next);

            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back);
            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back1);

            this.Bind(ViewModel,
                vm => vm.Progress,
                view => view.CheckingProgress.Value);
        }

        public CheckForUpdatesViewModel ViewModel
        {
            get { return (CheckForUpdatesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CheckForUpdatesViewModel), typeof(CheckForUpdatesView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CheckForUpdatesViewModel)value; }
        }
    }
}
