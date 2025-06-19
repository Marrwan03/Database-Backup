using System;
using System.ServiceProcess;

namespace MyFileMonitoringWinService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Running in Console Mode...");
                MyFileMonitoringWinService fileMonitoringWinService = new MyFileMonitoringWinService();
                fileMonitoringWinService.StartInControl();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new MyFileMonitoringWinService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            
        }
    }
}
