using Domain;

namespace Interfaces
{
    public interface IShowInformationClient
    {
        Show GetShow(ShowRequest showRequest);
    }
}