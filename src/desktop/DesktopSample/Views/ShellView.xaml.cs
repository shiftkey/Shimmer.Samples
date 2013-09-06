using System.Windows;
using ReactiveUI;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();
        }

        public ShellViewModel ViewModel {
            get { return (ShellViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ShellViewModel), typeof(ShellView), new PropertyMetadata(null));

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (ShellViewModel)value; }
        }
    }
}
