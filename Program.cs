using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Directline.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Directline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !Debugger.IsAttached && args.Contains("--service");

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--service").ToArray());
            var host = builder.Build();
            
            if(isService) {
                host.RunAsCustomService();
            } else {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging)=> {
                    logging.AddConsole();
                })
                .UseStartup<Startup>()
            .ConfigureLogging(options=> { options.AddConsole(); });
    }
}
