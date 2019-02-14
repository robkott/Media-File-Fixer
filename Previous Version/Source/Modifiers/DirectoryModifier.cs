using Domain;
using Interfaces;

namespace Modifiers
{
    public interface IDirectoryModifier : IModifier
    {

    }

    public class DirectoryModifier : IDirectoryModifier
    {
        public ModifyFileResult RenameAndMove(string fullyQualifiedPath)
        {
            throw new System.NotImplementedException();
        }
    }
}