using System;
using System.Text;

namespace Shared.Common.Helpers
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Extract most information about exception to string.
        /// </summary>
        public static string GetExceptionDetail(this Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Type name: " + exception.GetType());
            stringBuilder.AppendLine("Name: " + nameof(exception));
            stringBuilder.AppendLine("Default: " + exception);
            stringBuilder.AppendLine("Error: " + exception.Message);
            stringBuilder.AppendLine("Code: " + exception.HResult);
            stringBuilder.AppendLine("Default help: " + string.Format(!string.IsNullOrEmpty(exception.HelpLink) ? exception.HelpLink : "Unknown."));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Data: " + exception.Data);
            stringBuilder.AppendLine("TargetSite: " + exception.TargetSite);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Source: " + exception.Source);
            stringBuilder.AppendLine("StackTrace: " + exception.StackTrace);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("InnerException: " + string.Format(exception.InnerException != null ? GetExceptionDetail(exception.InnerException) : "Unknown."));
            return stringBuilder.ToString();
        }
    }
}