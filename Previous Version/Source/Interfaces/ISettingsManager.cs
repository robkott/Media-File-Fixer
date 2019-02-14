namespace Interfaces
{
    public interface ISettingsManager
    {
        string BaseTvPath { get; set; }
        string LastDirectoryOpened { get; set; }
    }
}