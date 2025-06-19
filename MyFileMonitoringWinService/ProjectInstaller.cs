using System.ComponentModel;
using System.ServiceProcess;

namespace MyFileMonitoringWinService
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
                ServiceName = "MyFileMonitoringWinService", // Must match the ServiceName in your ServiceBase class
                DisplayName = "My File Monitoring Win Service",
                Description = "A Service for Monitoring the Folder",
                StartType = ServiceStartMode.Automatic// Or Automatic, depending on requirements
            };

            // Add installers to the installer collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);

        }
    }
}
