using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class ForegroundUpdaterView : IViewFor<ForegroundUpdaterViewModel>
    {
        public ForegroundUpdaterView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel,
                vm => vm.NextCommand,
                view => view.Next);

            this.BindCommand(ViewModel,
                vm => vm.BackCommand,
                view => view.Back);
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
