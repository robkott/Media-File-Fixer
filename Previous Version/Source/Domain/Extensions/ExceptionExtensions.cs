using System;

namespace Domain.Extensions
{
    public static class ExceptionExtensions
    {
        public static string BuildErrorMessage(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            string msg = exception.Message + Environment.NewLine + exception.StackTrace;

            if (exception.InnerException != null)
            {
                msg += Environment.NewLine + Environment.NewLine + "INNER EXCEPTION" + Environment.NewLine;
                msg += exception.InnerException.BuildErrorMessage();
            }

            return msg;
        }
    }
}