using System.Diagnostics;
using System.Reflection;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Xaml;
using Squirrel.DesktopDemo.ViewModels;

namespace Squirrel.DesktopDemo.Views
{
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.SettingsCommand, x => x.Settings);
            this.BindCommand(ViewModel, x => x.UpdateBasicsCommand, x => x.Basics);
            this.BindCommand(ViewModel, x => x.BackgroundUpdaterCommand, x => x.Background);

            // what version are we currently running?

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version.Text = fvi.FileVersion;
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
