using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}