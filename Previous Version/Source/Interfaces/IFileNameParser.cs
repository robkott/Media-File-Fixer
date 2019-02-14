using Domain;

namespace Interfaces
{
    public interface IFileNameParser
    {
        ShowRequest ParseFileName(string fileName);
    }
}