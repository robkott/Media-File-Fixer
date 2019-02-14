using Domain;
using Interfaces;

namespace Modifiers
{
    public class Modifier : IModifier
    {
        private readonly IFileModifier _fileModifier;
        private readonly IDirectoryModifier _directoryModifier;

        public Modifier(IFileModifier fileModifier,
                        IDirectoryModifier directoryModifier)
        {
            _fileModifier = fileModifier;
            _directoryModifier = directoryModifier;
        }

        public ModifyFileResult RenameAndMove(string fullyQualifiedPath)
        {
            return _fileModifier.RenameAndMove(fullyQualifiedPath);
        }
    }
}