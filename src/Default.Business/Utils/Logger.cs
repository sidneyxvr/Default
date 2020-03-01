using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Default.Business.Utils
{
    public static class Logger
    {
        /// <summary>
        /// Write a log exception with request informations
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="context"></param>
        public static void WriteError(Exception exception, HttpContext context)
        {
            ///make sure this folder was created or uncoment the function below
            if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/wwwroot"))
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/wwwroot");
            }
            string path = $@"{Directory.GetCurrentDirectory()}/wwwroot/log";
            var line = new StackTrace(exception, true)?.GetFrame(0)?.GetFileLineNumber();
            using (StreamWriter sw = File.AppendText(path))
            {
                
                var query = string.Join(';', context.Request.Query?.Select(a => $" {a.Key}: {a.Value}")?.ToList());
                sw.WriteLine($"DateTime:  {DateTime.Now}");
                sw.WriteLine($"Path: {context.Request?.Path}");
                sw.WriteLine($"Query:{query}");
                sw.WriteLine($"Logged User: {context.User?.FindFirstValue(ClaimTypes.NameIdentifier)}");
                sw.WriteLine($"Line: {line}");
                if(context.Request.HasFormContentType)
                {
                    var data = string.Join(';', context.Request?.Form?.Select(a => $" {a.Key}: {a.Value}")?.ToList());
                    sw.WriteLine($"Data:{data}");
                }
                sw.WriteLine($"Exception Type: {exception.GetType().Name}");
                sw.WriteLine($"Message: {exception.Message}");
                sw.WriteLine($"Inner Exception: {exception.InnerException}");
                sw.WriteLine();
            }
        }

        /// <summary>
        /// Write a log exception
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteError(Exception exception)
        {
            string path = $@"{Directory.GetCurrentDirectory()}/wwwroot/log";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"DateTime:  {DateTime.Now}");
                sw.WriteLine($"Exception Type: {exception.GetType().Name}");
                sw.WriteLine($"Message: {exception.Message}");
                sw.WriteLine($"Inner Exception: {exception.InnerException}");
                sw.WriteLine();
            }
        }
    }
}
