using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace UserGroupSkill
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var hostUrl = args.Length > 1 ? args[1] : "http://*:33000";
            var server = WebApp.Start<Startup>(hostUrl);
            Trace.TraceInformation($"Alexa Skill Server running on {hostUrl}");

            var terminate = new AutoResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) => { terminate.Set(); };
            terminate.WaitOne();

            Trace.TraceInformation("Terminating Alexa Skill Server");
            server.Dispose();
        }
    }
}
