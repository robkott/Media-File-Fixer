using System;

namespace Domain
{
    public class ModifyFileResult
    {
        public string SourceFileName { get; set; }
        public string DestinationFileName { get; set; }
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}