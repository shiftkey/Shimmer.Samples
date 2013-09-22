using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.Logic;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class ForegroundUpdaterView : IViewFor<ForegroundUpdaterViewModel>
    {
        public ForegroundUpdaterView()
        {
            InitializeComponent();

            this.WhenAny(x => x.ViewModel.State, x => x.Value)
                .Where(x => x != null)
                .Subscribe(x => VisualStateManager.GoToElementState(Grid, x, true));

            this.BindCommand(ViewModel,
                vm => vm.NextCommand,
                view => view.Next);

            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back);
            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back1);
            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back2);
            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back3);

            this.Bind(ViewModel, 
                vm => vm.ErrorMessage,
                view => view.ErroMessage.Text);

            this.Bind(ViewModel,
                vm => vm.Progress,
                view => view.CheckingProgress.Value);
        }

        public ForegroundUpdaterViewModel ViewModel
        {
            get { return (ForegroundUpdaterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ForegroundUpdaterViewModel), typeof(ForegroundUpdaterView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ForegroundUpdaterViewModel)value; }
        }
    }
}
