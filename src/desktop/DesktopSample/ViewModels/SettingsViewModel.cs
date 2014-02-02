using System.IO;
using System.Reactive.Linq;
using System;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using Squirrel.DesktopDemo.Logic;

namespace Squirrel.DesktopDemo.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        readonly ObservableAsPropertyHelper<bool> _IsError;

        public SettingsViewModel(
            IScreen screen,
            ISettingsProvider settingsProvider,
            IFolderHelper folderHelper, 
            IAppContext appContext)
        {
            HostScreen = screen;

            BackCommand = new ReactiveAsyncCommand();
            BackCommand.RegisterAsyncAction(_ => HostScreen.Router.NavigateBack.Execute(null));

            SelectFolder = new ReactiveAsyncCommand();
            SelectFolder.RegisterAsyncAction(_ =>
            {
                var result = folderHelper.SelectFolder();
                if (result.Result == true) {
                    UpdateLocation = result.Folder;
                }
            }, appContext.DispatcherScheduler);

            UpdateLocation = settingsProvider.UpdateLocation;

            _IsError = this.WhenAny(vm => vm.UpdateLocation, vm => vm.Value)
                           .DistinctUntilChanged()
                           .Throttle(TimeSpan.FromMilliseconds(500))
                           .ObserveOn(appContext.DispatcherScheduler)
                           .Select(text => !IsUrlOrFolder(text))
                           .Do(error => {
                                if (!error) {
                                    settingsProvider.UpdateLocation = UpdateLocation;
                                }
                            })
                            .ToProperty(this, vm => vm.IsError, setViaReflection: false);
        }

        public bool IsError
        {
            get { return _IsError.Value; }
        }

        public bool IsUrlOrFolder(string arg)
        {
            ErrorMessage = "";
            if (String.IsNullOrWhiteSpace(arg)) {
                ErrorMessage = "please enter a path";
                return false;
            }

            Uri uri;
            if (Uri.TryCreate(arg, UriKind.RelativeOrAbsolute, out uri)) {
                if (uri.IsAbsoluteUri) {

                    if (uri.Scheme == "http" ||
                        uri.Scheme == "https") {
                        return true;
                    }

                    FileInfo fileInfo;
                    try {
                        
                        fileInfo = new FileInfo(arg);

                        if (fileInfo.Exists) {
                            ErrorMessage = "specify a directory, not a file";
                            return false;
                        }

                        if (!Path.IsPathRooted(arg)) {
                            ErrorMessage = "specify a full path";
                            return false;
                        }

                        if (!Directory.Exists(arg)) {
                            ErrorMessage = "the directory does not exist";
                            return false;
                        }

                        return true;
                    }
                    catch (ArgumentException) { }
                    catch (PathTooLongException) { }
                    catch (NotSupportedException) { }

                }
            }

            ErrorMessage = "i don't even know what this is";
            return false;
        }

        public ReactiveAsyncCommand BackCommand { get; set; }
        public ReactiveAsyncCommand SelectFolder { get; set; }

        string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { this.RaiseAndSetIfChanged(ref _ErrorMessage, value); }
        }

        string _UpdateLocation;
        public string UpdateLocation
        {
            get { return _UpdateLocation; }
            set { this.RaiseAndSetIfChanged(ref _UpdateLocation, value); }
        }

        public string UrlPathSegment { get { return "settings"; } }
        public IScreen HostScreen { get; private set; }
    }
}
