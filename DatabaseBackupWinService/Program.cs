using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackupWinService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new DatabaseBackupWinService()
            //};
            //ServiceBase.Run(ServicesToRun);

            if (Environment.UserInteractive)
            {
                Console.WriteLine("Running in Console Mode...");
                DatabaseBackupWinService databaseBackupWin = new DatabaseBackupWinService();
               databaseBackupWin.StartInControl();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new DatabaseBackupWinService()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
