namespace Interfaces
{
    public interface ITorrentClient
    {
        bool RemoveTorrent(string torrentHash);
    }
}