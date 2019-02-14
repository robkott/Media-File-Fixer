using Interfaces;

namespace Common
{
    public class SettingsManager : ISettingsManager
    {
        public string BaseTvPath
        {
            get { return Properties.Settings.Default.BaseTvPath; }
            set
            {
                Properties.Settings.Default.BaseTvPath = value;
                Properties.Settings.Default.Save();
            }
        }

        public string LastDirectoryOpened
        {
            get { return Properties.Settings.Default.LastDirectoryOpened; }
            set
            {
                Properties.Settings.Default.LastDirectoryOpened = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}