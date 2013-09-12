using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Shimmer.DesktopDemo.ViewModels;

namespace Shimmer.DesktopDemo.Views
{
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.SettingsCommand, x => x.Settings);
            this.BindCommand(ViewModel, x => x.UpdateBasicsCommand, x => x.Basics);
            this.BindCommand(ViewModel, x => x.BackgroundUpdaterCommand, x => x.Background);
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
