using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Squirrel.DesktopDemo.ViewModels;

namespace Squirrel.DesktopDemo.Views
{
    public partial class ErrorView : IViewFor<ErrorViewModel>
    {
        public ErrorView()
        {
            InitializeComponent();

            this.BindCommand(
                ViewModel,
                vm => vm.Back,
                view => view.Back);

            this.Bind(
                ViewModel,
                vm => vm.Message,
                view => view.ErrorMessage.Text);
        }

        public ErrorViewModel ViewModel
        {
            get { return (ErrorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ErrorViewModel), typeof(ErrorView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ErrorViewModel)value; }
        }
    }
}
