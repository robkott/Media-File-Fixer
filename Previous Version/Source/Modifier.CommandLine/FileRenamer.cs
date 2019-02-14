using Common;

namespace Modifier.CommandLine
{
    public class FileRenamer
    {
        public void RenameAndMove(string[] args)
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            var bootStrapper = new BootStrapper();
            bootStrapper.Run();
        }
    }
}