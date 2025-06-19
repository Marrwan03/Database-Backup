using System.ComponentModel;
using System.ServiceProcess;

namespace DatabaseBackupWinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {

        //C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();

            // Configure the Service Process Installer
            serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem // Adjust as needed (e.g., NetworkService, LocalService)
            };

            // Configure the Service Installer
            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "DatabaseBackupWinService", // Must match the ServiceName in your ServiceBase class
                DisplayName = "Database Backup Win Service",
                Description = "A Service for Backup the [HR_Database] Database.",
                StartType = ServiceStartMode.Automatic,// Or Automatic, depending on requirements
                ServicesDependedOn = new string[] {"RpcSs", "EventLog", "MSSQLSERVER"}
            };

            // Add installers to the installer collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);

        }
    }
}
