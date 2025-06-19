using BusinessTier;
using System;
using FileMonitoringLib;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DatabaseBackupWinService
{
    public partial class DatabaseBackupWinService : ServiceBase
    {
        string _ConnectionString;
        string _DatabaseBackups;
        string _BackupsFolder;
        string _LogFolder, _LogFileName;
        int? _BackupIntervalMinutes = null;

        string _GetMessage(string Title)
            => $"The {Title} was empty you should fill it in app.config";

        public DatabaseBackupWinService()
        {
            InitializeComponent();


            #region Initialize
            _ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            _DatabaseBackups = ConfigurationManager.AppSettings["DatabaseBackupsFolder"];
            _BackupsFolder = ConfigurationManager.AppSettings["BackupsFolder"];
            _LogFolder = ConfigurationManager.AppSettings["LogFolder"];
            _LogFileName = ConfigurationManager.AppSettings["LogFileName"];
            _BackupIntervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["BackupIntervalMinutes"]);
            #endregion

            #region Validation1
            if (string.IsNullOrEmpty(_LogFolder))
            {
                _LogFolder = @"C:\DatabaseBackups\Logs";
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("LogFolder"));
            }

            if (string.IsNullOrEmpty(_LogFileName))
            {
                _LogFileName = "Logging.txt";
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("LogFileName"));
            }
                

            if (string.IsNullOrEmpty(_DatabaseBackups))
            {
                _DatabaseBackups = @"C:\DatabaseBackups";
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("DatabaseBackups"));
            }
               

            if (string.IsNullOrEmpty(_BackupsFolder))
            {
                _BackupsFolder = @"C:\DatabaseBackups\Backups";
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("BackupsFolder"));
            }
                

            #endregion

            clsGlobal.LogFolder = _LogFolder;
            clsGlobal.LogFileName = _LogFileName;
            clsGlobal.BackupsFolder = new clsBackup(clsGlobal.LogFilePath(), _BackupsFolder, clsPillars.enType.Folder);
            clsGlobal.BackupsFolder.ActivatetheFileSystemWatcher();
            #region Created [Folder & File]
            try
            {
                clsUtil.CreateFor(_DatabaseBackups, clsPillars.enType.Folder);
                clsUtil.CreateFor(_BackupsFolder, clsPillars.enType.Folder);
                clsUtil.CreateFor(clsGlobal.LogFolder, clsPillars.enType.Folder);
                clsUtil.CreateFor(clsGlobal.LogFilePath(), clsPillars.enType.File);
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                    Console.WriteLine(ex.Message);
                throw;
            }




            #endregion

            #region Validation2          
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                _ConnectionString = "Server=.;Database=HR_Database;Integrated Security=True;";
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("ConnectionString"));
            }
            if (!_BackupIntervalMinutes.HasValue)
            {
                _BackupIntervalMinutes = 60;
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(),
                    _GetMessage("Backup Interval Minutes"));
            }
            #endregion
            clsGlobal.ConnectionString = _ConnectionString;
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
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath(), "Service started");

            #region Backup One time
            string DiskPath = Path.Combine(clsGlobal.BackupsFolder.path, "Backup_" + clsUtil.GenerateGUID() + ".bak");
            var BackUp = clsBackup.BackUp(DiskPath);
            if (!BackUp.Result)
            {
                clsUtil.LogServicesEvent(clsGlobal.LogFilePath(), $"Error during backup: {BackUp.MessageError}");
            }
            Task.Delay(3000);
            #endregion

            #region Backup Using Timer
            //TimeSpan tsTarget = new TimeSpan(0, _BackupIntervalMinutes.Value, 0);
            //TimeSpan tsBeginning = new TimeSpan(0, 0, 0);
            //DateTime dt = new DateTime(tsBeginning.Ticks);
            //while (true)
            //{
            //    dt = dt.AddSeconds(1);
            //    if (dt.Minute == tsTarget.Minutes && dt.Second == tsTarget.Seconds)
            //    {
            //        string DiskPath2 = Path.Combine(clsGlobal.BackupsFolder.path, "Backup_" + clsUtil.GenerateGUID() + ".bak");
            //        var BackUp2 = clsBackup.BackUp(DiskPath2);
            //        if (!BackUp2.Result)
            //        {
            //            clsUtil.LogServicesEvent(clsGlobal.LogFilePath(), $"Error during backup: {BackUp2.MessageError}");
            //        }
            //        dt = new DateTime(tsBeginning.Ticks);
            //        Task.Delay(3000);
            //    }

            //}
            #endregion

        }

        protected override void OnStop()
        {
            clsGlobal.BackupsFolder.DeActivatetheFileSystemWatcher();
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath(), "Service stopped");
        }
    }
}
