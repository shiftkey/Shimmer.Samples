using System.Windows;
using ReactiveUI;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class ForegroundUpdaterView : IViewFor<ForegroundUpdaterViewModel>
    {
        public ForegroundUpdaterView()
        {
            InitializeComponent();
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
