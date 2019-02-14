namespace Modifier.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileRenamer = new FileRenamer();
            fileRenamer.RenameAndMove(args);
        }
    }
}
