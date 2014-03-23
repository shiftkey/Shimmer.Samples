using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Squirrel.DesktopDemo.ViewModels;

namespace Squirrel.DesktopDemo.Views
{
    public partial class ApplyUpdatesView : IViewFor<ApplyUpdatesViewModel>
    {
        public ApplyUpdatesView()
        {
            InitializeComponent();

            this.WhenAny(x => x.ViewModel.State, x => x.Value)
                .Where(x => x != null)
                .Subscribe(x => VisualStateManager.GoToElementState(Grid, x, true));

            this.BindCommand(
                ViewModel,
                vm => vm.Apply,
                view => view.Apply);

            this.BindCommand(
                ViewModel,
                vm => vm.Restart,
                view => view.Restart);

            this.Bind(ViewModel,
                vm => vm.Progress,
                view => view.CheckingProgress.Value);
        }

        public ApplyUpdatesViewModel ViewModel
        {
            get { return (ApplyUpdatesViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ApplyUpdatesViewModel), typeof(ApplyUpdatesView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ApplyUpdatesViewModel)value; }
        }
    }
}
