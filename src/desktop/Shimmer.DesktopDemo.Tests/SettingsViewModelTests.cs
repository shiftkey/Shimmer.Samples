using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reactive.Concurrency;
using NSubstitute;
using ReactiveUI.Routing;
using Shimmer.DesktopDemo.Logic;
using Shimmer.DesktopDemo.ViewModels;
using Xunit;
using Xunit.Extensions;

namespace Shimmer.DesktopDemo.Tests
{
    public class SettingsViewModelTests
    {
        public class TheIsValidUrlMethod
        {
            IScreen screen = Substitute.For<IScreen>();
            ISettingsProvider settingsProvider = Substitute.For<ISettingsProvider>();
            IFolderHelper folderHelper = Substitute.For<IFolderHelper>();
            IAppContext appContext = Substitute.For<IAppContext>();
            IScheduler dispatcher = Substitute.For<IScheduler>();
            IFileSystem fileSystem = new MockFileSystem();

            [Theory]
            [InlineData("", false)]
            [InlineData("asfiosahfsaihfsaf", false)]
            [InlineData("www.google.com", false)]
            [InlineData("www.google", false)]
            [InlineData("http://www.google.com", false)]
            [InlineData("https://www.google.com", false)]
            [InlineData("http://www.google", false)]
            [InlineData("http://www.google", false)]
            public void TestAllTheUrls(string url, bool expected)
            {
                settingsProvider.UpdateLocation.Returns("");
                appContext.DispatcherScheduler.Returns(dispatcher);
                appContext.FileSystem.Returns(fileSystem);

                var viewModel = new SettingsViewModel(screen, settingsProvider, folderHelper, appContext);

                Assert.Equal(expected, viewModel.IsUrlOrFolder(url));
            }

            [Theory]
            [InlineData("", false)]
            public void TestAllTheFolders(string path, bool expected)
            {
                settingsProvider.UpdateLocation.Returns("");
                appContext.DispatcherScheduler.Returns(dispatcher);
                appContext.FileSystem.Returns(fileSystem);

                var viewModel = new SettingsViewModel(screen, settingsProvider, folderHelper, appContext);

                Assert.Equal(expected, viewModel.IsUrlOrFolder(path));
            }
        }
    }
}
