using System.Windows;
using ReactiveUI;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class SettingsView : IViewFor<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();

            this.Bind(ViewModel, vm => vm.UpdateLocation, v => v.UpdateLocation.Text);
        }

        public SettingsViewModel ViewModel
        {
            get { return (SettingsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SettingsViewModel), typeof(SettingsView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SettingsViewModel)value; }
        }
    }
}
