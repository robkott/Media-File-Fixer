using Domain;

namespace Interfaces
{
    public interface IModifier
    {
        ModifyFileResult RenameAndMove(string fullyQualifiedPath);
    }
}