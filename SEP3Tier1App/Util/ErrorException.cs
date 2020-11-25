using System;

namespace SEP3Tier1App.Util
{
    public class ErrorException : Exception
    {
        public ErrorException(string message) : base(message)
        {
            
        }
    }
}