using System;
using System.IO;
using System.Web;

namespace WebApplication2.Logger
{
    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~") + "log.txt", message + Environment.NewLine);
        }
    }
}