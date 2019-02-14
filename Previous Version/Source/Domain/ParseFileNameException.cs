using System;

namespace Domain
{
    public class ParseFileNameException : Exception
    {
        public ParseFileNameException(string fileName, string message)
            : base(message)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }
}