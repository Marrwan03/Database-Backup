using FileMonitoringLib;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;

namespace MyFileMonitoringWinService
{
    public partial class MyFileMonitoringWinService : ServiceBase
    {
        private clsPillars _DirectoryFolder;
        private clsPillars _LogFolder;
        private clsPillars _LogFile;

        public MyFileMonitoringWinService()
        {
            InitializeComponent();

            CanPauseAndContinue = true;
            CanShutdown = true;

            _DirectoryFolder = new clsPillars(ConfigurationManager.AppSettings["DirectoryFolder"], clsPillars.enType.Folder);
            clsUtil.CreateFor(_DirectoryFolder.path, _DirectoryFolder.Type);

            clsGlobal.SourceFolder = new clsSource(ConfigurationManager.AppSettings["SourceFolder"], clsPillars.enType.Folder);
            clsUtil.CreateFor(clsGlobal.SourceFolder.path, clsGlobal.SourceFolder.Type);


            clsGlobal.DestinationFolder = new clsDestination(ConfigurationManager.AppSettings["DestinationFolder"], clsPillars.enType.Folder);
            clsUtil.CreateFor(clsGlobal.DestinationFolder.path, clsGlobal.DestinationFolder.Type);


            _LogFolder = new clsPillars(ConfigurationManager.AppSettings["LogFolder"], clsPillars.enType.Folder);
            clsUtil.CreateFor(_LogFolder.path, _LogFolder.Type);
            _LogFile = new clsPillars(Path.Combine(_LogFolder.path, ConfigurationManager.AppSettings["LogFileName"]), clsPillars.enType.File);
            clsUtil.CreateFor(_LogFile.path, _LogFile.Type);

            clsGlobal.SourceFolder.ActivatetheFileSystemWatcher();
            clsGlobal.DestinationFolder.ActivatetheFileSystemWatcher();

        }

        public void StartInControl()
        {
            OnStart(null);
            Console.WriteLine("Please press any key to stop service...");
            Console.ReadLine();
            OnStop();
            Console.ReadKey();
        }

        protected override void OnStart(string[] args)
        {
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath, "Service Started");

        }

        protected override void OnStop()
        {
           clsGlobal.SourceFolder.DeActivatetheFileSystemWatcher();
           clsGlobal.DestinationFolder.DeActivatetheFileSystemWatcher();
           clsUtil.LogServicesEvent(clsGlobal.LogFilePath,"Service Stopped");

        }

        protected override void OnContinue()
        {
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath, "Service Resumed");
        }

        protected override void OnPause()
        {
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath, "Service Paused");
        }

        protected override void OnShutdown()
        {
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath, "Service Shut down due to System shutdown");
        }

    }
}
