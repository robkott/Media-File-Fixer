using Interfaces;

namespace ServiceClients
{
    public class UTorrentClient : ITorrentClient
    {
        #region Implementation of ITorrentClient

        public bool RemoveTorrent(string torrentHash)
        {
            return true;
        }

        #endregion
    }
}