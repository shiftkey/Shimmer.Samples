using Squirrel.DesktopDemo.Properties;

namespace Squirrel.DesktopDemo.Logic
{
    public interface ISettingsProvider
    {
        bool IsUpdateLocationSet { get; }
        string UpdateLocation { get; set; }
        string ApplicationName { get; }
    }

    public class SettingsProvider : ISettingsProvider
    {
        readonly Settings settings = Settings.Default;

        public bool IsUpdateLocationSet
        {
            get { return !string.IsNullOrWhiteSpace(settings.DefaultUpdateLocation); }
        }

        public string UpdateLocation
        {
            get { return settings.DefaultUpdateLocation; }
            set
            {
                settings.DefaultUpdateLocation = value;
                settings.Save();
            }
        }

        public string ApplicationName { get { return settings.ApplicationName; } }
    }
}
