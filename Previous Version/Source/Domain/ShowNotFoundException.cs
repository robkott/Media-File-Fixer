using System;

namespace Domain
{
    public class ShowNotFoundException : Exception
    {
        public string ShowName { get; set; }
    }
}