using System.Windows;
using ReactiveUI;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class BackgroundUpdaterView : IViewFor<BackgroundUpdaterViewModel>
    {
        public BackgroundUpdaterView()
        {
            InitializeComponent();
        }

        public BackgroundUpdaterViewModel ViewModel
        {
            get { return (BackgroundUpdaterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(BackgroundUpdaterViewModel), typeof(BackgroundUpdaterView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BackgroundUpdaterViewModel)value; }
        }
    }
}
